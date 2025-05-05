using StockMaster.Entities;

namespace StockMaster.Models
{
    public class SalesOrder : Order
    {
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
