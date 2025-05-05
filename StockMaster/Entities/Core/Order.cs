using StockMaster.Models;

namespace StockMaster.Entities
{
    public abstract class Order : BaseEntity
    {
        public DateTime Date { get; set; }
        public ICollection<ProductOrder> Products { get; set; } = [];
        public Invoice? Invoice { get; set; }
    }
}
