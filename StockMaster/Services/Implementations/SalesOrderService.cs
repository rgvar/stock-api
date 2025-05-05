using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using StockMaster.Data;
using StockMaster.Dtos;
using StockMaster.Entities;
using StockMaster.Exceptions;
using StockMaster.Models;

namespace StockMaster.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IMapper _mapper;
        private readonly StockDbContext _context;
        private readonly ILogger<SalesOrderService> _logger;
        private readonly IInvoiceService _invoiceService;

        public SalesOrderService(IMapper mapper, StockDbContext context, ILogger<SalesOrderService> logger, IInvoiceService invoiceService)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
            _invoiceService = invoiceService;
        }
        
        public async Task<OrderDto> Create(CreateOrderDto newOrderDto)
        {

            _logger.LogInformation("Creating new SalesOrder. ");

            var entity = _mapper.Map<SalesOrder>(newOrderDto);

            // Link the Contact to the Order
            var client = await _context.Clients.FindAsync(newOrderDto.ContactId);
            if (client is null)
            {
                _logger.LogError("Client ID: {id} was not found. ", newOrderDto.ContactId);
                throw new NotFoundException($"Client ID: {newOrderDto.ContactId} was not found. ");
            }
            entity.Client = client;

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

                if (product.StockQuantity < productOrder.Quantity)
                {
                    _logger.LogInformation("Insufficient stock. Available: {availableStock}. Requested: {requestedStock}.", product.StockQuantity, productOrder.Quantity);
                    throw new Exception("Stock is not enough. ");
                }

                product.StockQuantity -= productOrder.Quantity;

                ProductOrder newProductOrder = new()
                {
                    Product = product,
                    Quantity = productOrder.Quantity
                };

                _logger.LogInformation("Adding product ID: {id} \"{name}\" to the order. ", product.Id, product.Name);
                entity.Products.Add(newProductOrder);

            }
            
            entity.Client.SalesOrders.Add(entity);
            
            var savedEntity = await _context.SalesOrders.AddAsync(entity);

            await _context.SaveChangesAsync();


            if (newOrderDto.InvoiceData is not null)
            {
                var newInvoiceDto = _mapper.Map<CreateInvoiceDto>(newOrderDto.InvoiceData);
                newInvoiceDto.OrderId = savedEntity.Entity.Id;
                await _invoiceService.Create(newInvoiceDto);
            }

            var orderDto = _mapper.Map<OrderDto>(savedEntity.Entity);
            
            _logger.LogInformation("Sales Order ID: {id} created successfully. ", savedEntity.Entity.Id);

            return orderDto;

        }


        public async Task Delete(int id)
        {
            _logger.LogInformation("Deleting Order ID: {id}. ", id);

            var result = await _context.SalesOrders.FindAsync(id);
            if (result is null)
            {
                _logger.LogError("Order ID: {entityId} was not found. ", id);
                throw new NotFoundException($"Order ID: {id} was not found. ");
            }

            _context.SalesOrders.Remove(result);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Order ID: {id} deleted successfully. ", id);

        }

        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            _logger.LogInformation("Retrieving Orders. ");

            var orders = await _context.SalesOrders
                .AsNoTracking()
                .Include(so => so.Products)
                .Include(so => so.Invoice)
                .Include(so => so.Client)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            _logger.LogInformation("Retrieved {count} Orders. ", orders.Count);

            return orders;
        }

        public async Task<OrderDto> GetById(int id)
        {
            _logger.LogInformation("Retrieving Order ID: {id}. ", id);

            var order = await _context.SalesOrders
                .AsNoTracking()
                .Include(so => so.Products)
                .Include(so => so.Invoice)
                    .ThenInclude(i => i!.PaymentType)
                .Include(so => so.Client)
                .FirstOrDefaultAsync(so => so.Id == id);

            if (order is null)
            {
                _logger.LogError("Order ID: {entityId} was not found. ", id);
                throw new NotFoundException($"Order ID: {id} was not found. ");
            }

            var orderDto = _mapper.Map<OrderDto>(order);

            _logger.LogInformation("Retrieved Order ID: {id} successfully. ", id);

            return orderDto;

        }

        public async Task Update(int id, OrderDto updatedDto)
        {
            _logger.LogInformation("Updating Order ID: {id}. ", id);
            if (id != updatedDto.Id)
            {
                _logger.LogError("Given ID: {id} does not match with the Order ID: {poId}. ", id, updatedDto.Id);
                throw new BadHttpRequestException($"Given ID: {id} does not match with the Order ID: {updatedDto.Id}. ");
            }

            var existingOrder = await _context.SalesOrders.FindAsync(id);
            if (existingOrder is null)
            {
                _logger.LogError("Order ID: {id} does not exist. ", id);
                throw new BadHttpRequestException($"Order ID: {id} does not exist. ");
            }

            var newOrder = _mapper.Map<SalesOrder>(updatedDto);

            _context.SalesOrders.Update(newOrder);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order ID: {id} saved successfully. ", id);

        }
    }
}
