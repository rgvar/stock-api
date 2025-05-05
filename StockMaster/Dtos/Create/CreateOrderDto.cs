using StockMaster.Enums;

namespace StockMaster.Dtos
{
    public class CreateOrderDto
    {
        public required List<ProductOrderDto> Products { get; set; } = [];
        public required int ContactId { get; set; }
        public InvoiceData? InvoiceData { get; set; }
    }
}
