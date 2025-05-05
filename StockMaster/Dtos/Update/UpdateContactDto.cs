using System.ComponentModel.DataAnnotations;

namespace StockMaster.Dtos
{
    public class UpdateContactDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters. ")]
        public required string Name { get; set; }
        [Phone]
        [MaxLength(20, ErrorMessage = "Phone Number must be less than 20 numbers. ")]
        public string? PhoneNumber { get; set; }
        [MaxLength(80, ErrorMessage = "Address must be less than 80 characters. ")]
        public string? Address { get; set; }
    }
}
