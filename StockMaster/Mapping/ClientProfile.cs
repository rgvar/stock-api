using AutoMapper;
using StockMaster.Dtos;
using StockMaster.Entities;
using StockMaster.Models;

namespace StockMaster.Mapping
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ContactDto>()
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.SalesOrders));

            CreateMap<ContactDto, Client>();

            CreateMap<CreateContactDto, Client>();

            CreateMap<Client, ContactSimpleDto>();

        }
    }
}
