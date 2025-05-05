using StockMaster.Dtos;

namespace StockMaster.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetById(int id);
        Task<IEnumerable<ProductDto>> GetAll(string? search);
        Task<ProductDto> Create(CreateProductDto newProduct);
        Task Update(int id, UpdateProductDto toUpdateProduct);
        Task Delete(int id);
    }
}
