using StockMaster.Dtos;

namespace StockMaster.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategorySimpleDto>> GetAll();
        public Task<CategoryDto> GetWithProductsById(int id);
        public Task<CategorySimpleDto> GetById(int id);
        public Task<CategorySimpleDto> Create(CreateCategoryDto newCategory);
        public Task Delete(int id);
        public Task Update(int id, CategorySimpleDto updatedCategory);
    }
}
