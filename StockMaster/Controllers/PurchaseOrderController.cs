using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMaster.Dtos;
using StockMaster.Models;
using StockMaster.Services;

namespace StockMaster.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        public PurchaseOrderController(IPurchaseOrderService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetAllPurchaseOrder")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllPurchaseOrders()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetPurchaseOrderById")]
        public async Task<ActionResult<OrderDto>> GetPurchaseOrderById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "CreatePurchaseOrder")]
        public async Task<ActionResult<OrderDto>> CreatePurchaseOrden([FromBody] CreateOrderDto newPurchaseOrderDto)
        {
            var result = await _service.Create(newPurchaseOrderDto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdatePurchaseOrder")]
        public async Task<ActionResult> UpdatePurchaseOrder(int id, [FromBody] OrderDto updatedPurchaseOrder)
        {
            await _service.Update(id, updatedPurchaseOrder);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeletePurchaseOrder")]
        public async Task<ActionResult> DeletePurchaseOrder(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
