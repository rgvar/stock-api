namespace StockMaster.Dtos
{
    public class OrderForInvoiceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ProductOrderDto> Products { get; set; } = [];
    }
}
