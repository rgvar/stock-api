namespace StockMaster.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ICollection<ProductDto> Products { get; set; } = [];
    }
}
