using StockMaster.Dtos;
using StockMaster.Entities;

namespace StockMaster.Services
{
    public interface ISalesOrderService
    {
        Task<IEnumerable<OrderDto>> GetAll();
        Task<OrderDto> GetById(int id);
        Task<OrderDto> Create(CreateOrderDto createOrderDto);
        Task Update(int id, OrderDto updatedDto);
        Task Delete(int id);
    }
}
