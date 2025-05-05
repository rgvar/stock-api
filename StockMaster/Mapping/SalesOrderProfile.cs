using AutoMapper;
using StockMaster.Dtos;
using StockMaster.Models;

namespace StockMaster.Mapping
{
    public class SalesOrderProfile : Profile
    {
        public SalesOrderProfile()
        {
            CreateMap<OrderDto, SalesOrder>();

            CreateMap<SalesOrder, OrderDto>()
                .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Client));

            CreateMap<CreateOrderDto, SalesOrder>()
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<SalesOrder, OrderForInvoiceDto>();

            CreateMap<SalesOrder, OrderForContactDto>();
        }
    }
}
