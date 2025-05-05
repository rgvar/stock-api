using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StockMaster.Dtos.Create;
using StockMaster.Services.Interfaces;
using StockMaster.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;
        public AuthController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var user = await _service.Authenticate(login);
            var token = _service.GenerateJwtToken(user);

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] CreateUserDto createUserDto)
        {
            var user = await _service.CreateUser(createUserDto);
            var token = _service.GenerateJwtToken(user);

            return Ok(new {token});
        }

        
    }
}
