using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StockMaster.Data;
using StockMaster.Dtos;
using StockMaster.Entities;
using StockMaster.Services.Interfaces;
using StockMaster.Exceptions;
using StockMaster.Enums;
using StockMaster.Models;

namespace StockMaster.Services.Implementations
{
    public class InvoiceService : IInvoiceService
    {

        private readonly StockDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceService> _logger;
        public InvoiceService(StockDbContext context, IMapper mapper, ILogger<InvoiceService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<InvoiceDto>> GetAll()
        {
            _logger.LogInformation("Retrieving Invoices. ");

            var invoices = await _context.Invoices
                .ProjectTo<InvoiceDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            _logger.LogInformation("Retrieved {count} invoices. ", invoices.Count);

            return invoices;

        }

        public async Task<InvoiceDto> GetById(int id)
        {
            _logger.LogInformation("Retrieving Invoice ID: {id}. ", id);

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice is null)
            {
                _logger.LogInformation("Invoice ID: {entityId} was not found. ", id);
                throw new NotFoundException($"Invoice ID: {id} was not found. ");
            }

            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);

            _logger.LogInformation("Invoice ID: {id} retrieved successfully. ", id);

            return invoiceDto;
        }

        public async Task<InvoiceDto> Create(CreateInvoiceDto createInvoiceDto)
        {

            _logger.LogInformation("Creating Invoice for Order ID: {orderId}. ", createInvoiceDto.OrderId);
            

            var invoice = _mapper.Map<Invoice>(createInvoiceDto);

            if (createInvoiceDto.ContactType is ContactType.Client)
            {
                invoice.Order = await _context.SalesOrders
                    .Include(o => o.Invoice)
                    .FirstOrDefaultAsync(o => o.Id == createInvoiceDto.OrderId);
            } else if (createInvoiceDto.ContactType is ContactType.Supplier)
            {
                invoice.Order = await _context.PurchaseOrders
                    .Include(o => o.Invoice)
                    .FirstOrDefaultAsync(o => o.Id == createInvoiceDto.OrderId);
            }

            if (invoice.Order is null)
            {
                _logger.LogError("Order ID: {OrderId} was not found. ", createInvoiceDto.OrderId);
                throw new NotFoundException($"Order ID: {createInvoiceDto.OrderId} was not found. ");
            }
            
            if (invoice.Order.Invoice is not null)
            {
                _logger.LogError("Order ID: {OrderId} already has an associated Invoice. If you want to modify it use Update. ", createInvoiceDto.OrderId);
                throw new Exception($"Order ID: {createInvoiceDto.OrderId} already has an associated Invoice. If you want to modify it use Update. ");
            }

            invoice.PaymentType = await _context.PaymentTypes.FindAsync(createInvoiceDto.PaymentTypeId);

            if (invoice.PaymentType is null)
            {
                _logger.LogError("PaymentType ID: {PaymentTypeId} was not found. ", createInvoiceDto.PaymentTypeId);
                throw new NotFoundException($"PaymentType ID: {createInvoiceDto.PaymentTypeId} was not found. ");
            }

            invoice.Code = CreateInvoiceCode(invoice);
            invoice.Subtotal = GetSubtotal(invoice.Order);
            invoice.Total = CalculateTotal(invoice);

            if (invoice.Code is null)
            {
                _logger.LogError("The code can not be null. ");
                throw new Exception("The code can not be null. ");
            }

            var savedInvoice = await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            if (createInvoiceDto.ContactType is ContactType.Client)
            {
                var order = invoice.Order as SalesOrder;
                if (order is null)
                {
                    _logger.LogError("Order was not selected correctly for Invoice ID: {invoiceId}. ", invoice.Id);
                    throw new NotFoundException($"Order was not selected correctly for Invoice ID: {invoice.Id}. ");
                }
                order.Invoice = savedInvoice.Entity;
            }
            else if (createInvoiceDto.ContactType is ContactType.Supplier)
            {
                var order = invoice.Order as SalesOrder;
                if (order is null)
                {
                    _logger.LogError("Order was not selected correctly for Invoice ID: {invoiceId}. ", invoice.Id);
                    throw new NotFoundException($"Order was not selected correctly for Invoice ID: {invoice.Id}. ");
                }
                order.Invoice = savedInvoice.Entity;
            }
            await _context.SaveChangesAsync();

            var invoiceDto = _mapper.Map<InvoiceDto>(savedInvoice.Entity);

            _logger.LogInformation("Invoice saved successfully. ");

            return invoiceDto;
        }

        public async Task Delete(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice is null)
            {
                _logger.LogInformation("Invoice ID: {entityId} was not found. ", id);
                throw new NotFoundException($"Invoice ID: {id} was not found. ");
            }
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

        }

        public async Task Update(int id, InvoiceDto updatedDto)
        {
            if (id != updatedDto.Id)
            {
                _logger.LogError("Category ID mismatch. Given ID: {givenId}, Expected ID: {expectedId}. ", updatedDto.Id, id);
                throw new BadHttpRequestException("Category ID mismatch. ");
            }
            var updatedEntity = _mapper.Map<Invoice>(updatedDto);
            _context.Invoices.Update(updatedEntity);

            await _context.SaveChangesAsync();
        }


        private string CreateInvoiceCode(Invoice invoice)
        {
            string code = string.Join("-",
                invoice.ContactType.ToString().First(),
                invoice.PaymentType!.Short,
                invoice.Id.ToString().PadLeft(4, '0'));

            _logger.LogInformation("Generated Code: {code}", code);

            return code;
        }

        private decimal GetSubtotal(Order order)
        {
            decimal subtotal = 0;
            foreach (ProductOrder po in order.Products)
            {
                decimal productSubtotal = po.Product.Price * po.Quantity;
                subtotal += productSubtotal;

                _logger.LogInformation("{productName} added to subtotal for US${sub}. ", po.Product.Name, subtotal);
            }

            _logger.LogInformation("Subtotal: {subtotal}. ", subtotal);

            return subtotal;
        }

        private decimal CalculateTotal(Invoice invoice)
        {
            decimal taxes = invoice.Subtotal * invoice.Taxes / 100;
            decimal discount = 1 - invoice.Discount / 100;

            decimal total = (invoice.Subtotal + taxes) * discount;

            _logger.LogInformation("Total: {total}. ", total);

            return total;
        }
    }
}
