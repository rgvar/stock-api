using StockMaster.Services;
using Microsoft.AspNetCore.Mvc;
using StockMaster.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace StockMaster.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _service;
        public InvoiceController(IInvoiceService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetAllInvoices")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAllInvoices()
        {
            var result = await _service.GetAll();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetInvoiceById")]
        public async Task<ActionResult<InvoiceDto>> GetInvoiceById(int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "CreateInvoice")]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateInvoiceDto createInvoiceDto)
        {
            var result = await _service.Create(createInvoiceDto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateInvoice")]
        public async Task<ActionResult> UpdateInvoice(int id, [FromBody] InvoiceDto updatedDto)
        {
            await _service.Update(id, updatedDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteInvoice")]
        public async Task<ActionResult> DeleteInvoice(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }

    }
}
