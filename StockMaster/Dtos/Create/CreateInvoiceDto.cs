using StockMaster.Entities;
using StockMaster.Enums;

namespace StockMaster.Dtos
{
    public class CreateInvoiceDto
    {
        public int Discount { get; set; } = 0;
        public int Taxes { get; set; } = 0;
        public bool IsPaid { get; set; }
        public ContactType ContactType { get; set; }
        public required int PaymentTypeId { get; set; }
        public required int OrderId { get; set; }
    }
}
