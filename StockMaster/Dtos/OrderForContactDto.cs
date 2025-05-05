namespace StockMaster.Dtos
{
    public class OrderForContactDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ProductOrderDto> Products { get; set; } = [];
        public InvoiceSimpleDto? Invoice { get; set; }
    }
}
