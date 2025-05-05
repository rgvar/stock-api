using StockMaster.Entities;

namespace StockMaster.Models
{
    public class Supplier : Contact
    {
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = [];
    }
}
