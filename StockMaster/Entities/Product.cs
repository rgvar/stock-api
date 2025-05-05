using StockMaster.Entities;
using System.ComponentModel.DataAnnotations;

namespace StockMaster.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name must be less than 100 characters. ")]
        public required string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; } = 0;
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        public List<Category> Categories { get; set; } = [];

    }
}
