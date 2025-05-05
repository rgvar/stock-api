using StockMaster.Dtos;

namespace StockMaster.Services
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<OrderDto>> GetAll();
        Task<OrderDto> GetById(int id);
        Task<OrderDto> Create(CreateOrderDto createOrderDto);
        Task Update(int id, OrderDto updatedDto);
        Task Delete(int id);
    }
}
