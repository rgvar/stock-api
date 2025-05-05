using System.ComponentModel.DataAnnotations;

namespace StockMaster.Dtos
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name must be less than 100 characters. ")]
        public required string Name { get; set; }
        [MaxLength(500, ErrorMessage = "Max 500 characters for the description. ")]
        public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }
        public List<int> CategoriesId { get; set; } = [];
    }
}
