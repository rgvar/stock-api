using Microsoft.EntityFrameworkCore;
using StockMaster.Data;
using StockMaster.Services.Interfaces;
using StockMaster.Users;
using Azure.Identity;
using StockMaster.Dtos.Create;
using AutoMapper;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace StockMaster.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly StockDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(ILogger<UserService> logger, StockDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<User> Authenticate(LoginModel login)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == login.Username);
            if (user is null)
            {
                _logger.LogError("The given username does not exist. ");
                throw new AuthenticationFailedException("The given username does not exist. ");
            }
            
            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                _logger.LogError("Incorrect password. ");
                throw new AuthenticationFailedException("Incorrect password. ");
            }

            return user;
        }

        public async Task<User> CreateUser(CreateUserDto newUser)
        {
            var user = _mapper.Map<User>(newUser);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
            
            _context.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("user created: {user}", user.ToString());

            return user;

        }

        public object GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials

            );

            _logger.LogInformation("Token created {token}", token.ToString());

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
