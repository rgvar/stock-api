namespace StockMaster.Entities
{
    public class PaymentType
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Short { get; set; }
    }
}
