using StockMaster.Models;
using System.ComponentModel.DataAnnotations;

namespace StockMaster.Entities
{
    public class ProductOrder
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
    }
}
