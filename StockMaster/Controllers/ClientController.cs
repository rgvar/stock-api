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
    public class ClientController : ControllerBase
    {
        private readonly IContactService<Client> _service;
        public ClientController(IContactService<Client> service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetAllClients")]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetAllClients()
        {
            var clients = await _service.GetContacts();
            return Ok(clients);
        }

        [HttpGet("{id}", Name = "GetClientById")]
        public async Task<ActionResult<ContactDto>> GetClientById(int id)
        {
            var client = await _service.GetContactById(id);
            return Ok(client);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "CreateClient")]
        public async Task<ActionResult<ContactDto>> CreateClient([FromBody] CreateContactDto clientDto)
        {
            var client = await _service.CreateContact(clientDto);
            return Ok(client);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "UpdateClient")]
        public async Task<ActionResult> UpdateClient(int id, [FromBody] ContactDto clientDto)
        {
            await _service.UpdateContact(id, clientDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteClient")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            await _service.RemoveContact(id);
            return NoContent();
        }
        
    }
}
