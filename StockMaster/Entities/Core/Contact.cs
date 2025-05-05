using StockMaster.Models;
using System.ComponentModel.DataAnnotations;

namespace StockMaster.Entities
{
    public abstract class Contact : BaseEntity
    {
        [MaxLength(50)]
        public required string Name { get; set; }
        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        [MaxLength(80)]
        public string? Address { get; set; }
    }
}
