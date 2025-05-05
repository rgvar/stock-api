using StockMaster.Entities;

namespace StockMaster.Models
{
    public class PurchaseOrder : Order
    {
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
