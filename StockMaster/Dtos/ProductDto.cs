using System.ComponentModel.DataAnnotations;

namespace StockMaster.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Name must be less than 100 characters. ")]
        public required string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public ICollection<CategorySimpleDto> Categories { get; set; } = [];
    }
}
