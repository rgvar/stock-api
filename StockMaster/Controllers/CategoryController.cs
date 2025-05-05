using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockMaster.Dtos;
using StockMaster.Services.Interfaces;

namespace StockMaster.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategorySimpleDto>>> GetAllCategories()
        {
            var categories = await _service.GetAll();
            return Ok(categories);
        }

        [HttpGet("{id}/products", Name = "GetCategoryWithProducts")]
        public async Task<ActionResult<CategoryDto>> GetCategoryWithProducts(int id)
        {
            var category = await _service.GetWithProductsById(id);
            return Ok(category);
        }

        [HttpGet("{id}",Name = "GetCategory")]
        public async Task<ActionResult<CategorySimpleDto>> GetCategory(int id)
        {
            var category = await _service.GetById(id);
            return Ok(category);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "CreateCategory")]
        public async Task<ActionResult<CategorySimpleDto>> CreateCategory([FromBody] CreateCategoryDto newCategory)
        {
            var category = await _service.Create(newCategory);
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteCategory")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateCategory")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategorySimpleDto updatedCategory)
        {
            await _service.Update(id, updatedCategory);
            return NoContent();
            
        }
    }
}
