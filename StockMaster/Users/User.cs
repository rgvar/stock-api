using StockMaster.Entities;

namespace StockMaster.Users
{
    public class User : BaseEntity
    {
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }
        public string? Email { get; set; }

    }
}
