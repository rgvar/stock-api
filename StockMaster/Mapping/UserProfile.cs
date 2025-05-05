using AutoMapper;
using StockMaster.Dtos.Create;
using StockMaster.Users;

namespace StockMaster.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        }
    }
}
