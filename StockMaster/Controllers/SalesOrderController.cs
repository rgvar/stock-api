using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using StockMaster.Dtos;
using StockMaster.Models;
using StockMaster.Services;

namespace StockMaster.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrderService _service;
        public SalesOrderController(ISalesOrderService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetAllSalesOrder")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllSalesOrders()
        {
            var order = await _service.GetAll();
            return Ok(order);
        }

        [HttpGet("{id}",Name = "GetSalesOrderById")]
        public async Task<ActionResult<OrderDto>> GetSalesOrderById(int id)
        {
            var order = await _service.GetById(id);
            return Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "CreateSalesOrder")]
        public async Task<ActionResult<OrderDto>> CreateSalesOrder([FromBody] CreateOrderDto newSalesOrderDto)
        {
            var order = await _service.Create(newSalesOrderDto);
            return Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateSalesOrder")]
        public async Task<ActionResult> UpdateSalesOrder(int id, [FromBody] OrderDto updatedSalesOrder)
        {
            await _service.Update(id, updatedSalesOrder);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteSalesOrder")]
        public async Task<ActionResult> DeleteSalesOrder(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
