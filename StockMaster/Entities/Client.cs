using StockMaster.Entities;

namespace StockMaster.Models
{
    public class Client : Contact
    {
        public ICollection<SalesOrder> SalesOrders { get; set; } = [];
    }
}
