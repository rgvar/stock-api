using StockMaster.Entities;
using StockMaster.Enums;

namespace StockMaster.Entities
{
    public class Invoice : BaseEntity
    {
        public string? Code { get; set; }
        public DateTime Date { get; set; }
        public decimal Subtotal { get; set; }
        public int Discount { get; set; } = 0;
        public int Taxes { get; set; } = 0;
        public decimal Total { get; set; }
        public bool IsPaid { get; set; }
        public ContactType ContactType { get; set; }

        public int? PaymentTypeId { get; set; }
        public PaymentType? PaymentType { get; set; }

        public int? OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
