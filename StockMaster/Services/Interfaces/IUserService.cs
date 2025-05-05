using StockMaster.Dtos.Create;
using StockMaster.Users;

namespace StockMaster.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(LoginModel login);
        Task<User> CreateUser(CreateUserDto newUser);
        object GenerateJwtToken(User user);
    }
}
