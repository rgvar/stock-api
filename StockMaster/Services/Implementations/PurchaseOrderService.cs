using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StockMaster.Data;
using StockMaster.Dtos;
using StockMaster.Entities;
using StockMaster.Exceptions;
using StockMaster.Models;

namespace StockMaster.Services.Implementations
{
    public class PurchaseOrderService : IPurchaseOrderService
    {

        private readonly IMapper _mapper;
        private readonly StockDbContext _context;
        private readonly ILogger<PurchaseOrderService> _logger;

        public PurchaseOrderService(IMapper mapper, StockDbContext context, ILogger<PurchaseOrderService> logger)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<OrderDto> Create(CreateOrderDto newOrderDto)
        {
            _logger.LogInformation("Creating Purchase Order for {productsCount} products. ", newOrderDto.Products.Count);
            var entity = _mapper.Map<PurchaseOrder>(newOrderDto);

            // Link the Contact to the Order
            _logger.LogInformation("Searching for Supplier ID: {supplierId}", newOrderDto.ContactId);
            var supplier = await _context.Suppliers.FindAsync(newOrderDto.ContactId);
            if (supplier is null)
            {
                _logger.LogError("Supplier ID: {supplierId} was not found. ", newOrderDto.ContactId);
                throw new NotFoundException($"Supplier ID: {newOrderDto.ContactId} was not found. ");
            }
            _logger.LogInformation("Supplier ID: {supplierId} \"{supplierName}\" found. ", supplier.Id, supplier.Name);
            entity.Supplier = supplier;

            // Link the Products to the Order
            foreach (ProductOrderDto productOrder in newOrderDto.Products)
            {
                var product = await _context.Products.FindAsync(productOrder.ProductId);
                if (product is null)
                {
                    _logger.LogError("Product ID: {productId} was not found. ", productOrder.ProductId);
                    throw new NotFoundException($"Product ID: {productOrder.ProductId} was not found. ");
                }

                _logger.LogInformation("Found product ID: {productId}, Name: {productName}", product.Id, product.Name);

                product.StockQuantity += productOrder.Quantity;

                ProductOrder newProductOrder = new()
                {
                    Product = product,
                    Quantity = productOrder.Quantity
                };

                _logger.LogInformation("Adding product ID: {id} \"{name}\" to the order. ", product.Id, product.Name);
                entity.Products.Add(newProductOrder);

            }


            entity.Supplier.PurchaseOrders.Add(entity);

            var savedEntity = await _context.PurchaseOrders.AddAsync(entity);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Purchase Order ID: {id} created successfully. ", savedEntity.Entity.Id);

            var orderDto = _mapper.Map<OrderDto>(savedEntity.Entity);
            return orderDto;

        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Deleting Order ID: {id}. ", id);

            var result = await _context.PurchaseOrders.FindAsync(id);
            if (result is null)
            {
                _logger.LogError("Order ID: {entityId} was not found. ", id);
                throw new NotFoundException($"Order ID: {id} was not found. ");
            }

            _context.PurchaseOrders.Remove(result);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Order ID: {id} deleted successfully. ", id);
        }

        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            _logger.LogInformation("Retrieving Orders. ");

            var orders = await _context.PurchaseOrders
                .AsNoTracking()
                .Include(po => po.Products)
                .Include(po => po.Invoice)
                .Include(po => po.Supplier)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            _logger.LogInformation("Retrieved {count} Orders. ", orders.Count);

            return orders;
        }

        public async Task<OrderDto> GetById(int id)
        {
            _logger.LogInformation("Retrieving Order ID: {id}. ", id);

            var result = await _context.PurchaseOrders
                .AsNoTracking()
                .Include(po => po.Products)
                .Include(so => so.Invoice)
                .Include(po => po.Supplier)
                .FirstOrDefaultAsync(po => po.Id == id);

            if (result is null)
            {
                _logger.LogError("Order ID: {entityId} was not found. ", id);
                throw new NotFoundException($"Order ID: {id} was not found. ");
            }

            var dto = _mapper.Map<OrderDto>(result);

            _logger.LogInformation("Retrieved Order ID: {id} successfully. ", id);

            return dto;

        }

        public async Task Update(int id, OrderDto updatedDto)
        {
            _logger.LogInformation("Updating Order ID: {id}. ", id);
            if (id != updatedDto.Id)
            {
                _logger.LogError("Given ID: {id} does not match with the Order ID: {poId}. ", id, updatedDto.Id);
                throw new BadHttpRequestException($"Given ID: {id} does not match with the Order ID: {updatedDto.Id}. ");
            }

            var existingOrder = await _context.PurchaseOrders.FindAsync(id);
            if (existingOrder is null)
            {
                _logger.LogError("Order ID: {id} does not exist. ", id);
                throw new BadHttpRequestException($"Order ID: {id} does not exist. ");
            }

            var newOrder = _mapper.Map<PurchaseOrder>(updatedDto);

            _context.PurchaseOrders.Update(newOrder);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order ID: {id} saved successfully. ", id);

        }
    }
}
