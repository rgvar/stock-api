using StockMaster.Entities;
using StockMaster.Enums;

namespace StockMaster.Dtos
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public DateTime Date { get; set; }
        public decimal Subtotal { get; set; }
        public int Discount { get; set; } = 0;
        public int Taxes { get; set; } = 0;
        public decimal Total { get; set; }
        public bool IsPaid { get; set; }
        public OrderForInvoiceDto? Order { get; set; }
        public ContactType ContactType { get; set; }
        public required PaymentType PaymentType { get; set; }
    }
}
