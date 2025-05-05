using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMaster.Dtos;
using StockMaster.Services.Interfaces;

namespace StockMaster.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts([FromQuery] string? search = null)
        {
            var products = await _service.GetAll(search);
            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _service.GetById(id);
            return Ok(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "CreateProduct")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto newProductDto)
        {

            var product = await _service.Create(newProductDto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteProduct")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateProduct")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updatedProductDto)
        {
            await _service.Update(id, updatedProductDto);
            return NoContent();
        }

    }
}
