namespace StockMaster.Dtos
{
    public record ProductOrderDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
