using Microsoft.AspNetCore.Identity;

namespace StockMaster.Dtos.Create
{
    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public string? Email { get; set; }
    }
}
