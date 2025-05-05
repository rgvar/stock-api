using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockMaster.Dtos;
using StockMaster.Models;
using StockMaster.Services.Interfaces;

namespace StockMaster.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IContactService<Supplier> _service;
        public SupplierController(IContactService<Supplier> service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetAllSuppliers")]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetAllSuppliers()
        {
            var suppliers = await _service.GetContacts();
            return Ok(suppliers);
        }

        [HttpGet("{id}", Name = "GetSupplierById")]
        public async Task<ActionResult<ContactDto>> GetSupplierById(int id)
        {
            var supplier = await _service.GetContactById(id);
            return Ok(supplier);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "CreateSupplier")]
        public async Task<ActionResult<ContactDto>> CreateSupplier([FromBody] CreateContactDto supplierDto)
        {
            var supplier = await _service.CreateContact(supplierDto);
            return Ok(supplier);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateSupplier")]
        public async Task<ActionResult> UpdateSupplier(int id, [FromBody] ContactDto supplierDto)
        {
            await _service.UpdateContact(id, supplierDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteSupplier")]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            await _service.RemoveContact(id);
            return NoContent();
        }
    }
}
