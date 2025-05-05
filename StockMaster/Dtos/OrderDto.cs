namespace StockMaster.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ProductOrderDto> Products { get; set; } = [];
        public InvoiceSimpleDto? Invoice { get; set; }
        public ContactSimpleDto? Contact { get; set; }
    }
}
