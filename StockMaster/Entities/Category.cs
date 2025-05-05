using StockMaster.Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace StockMaster.Models
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        public ICollection<Product> Products { get; set; } = [];
    }
}
