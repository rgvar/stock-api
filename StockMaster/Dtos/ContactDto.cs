using StockMaster.Entities;
using StockMaster.Models;
using System.ComponentModel.DataAnnotations;

namespace StockMaster.Dtos
{
    public class ContactDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public List<OrderForContactDto>? Orders { get; set; } = [];
    }
}
