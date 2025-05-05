using AutoMapper;
using StockMaster.Dtos;
using StockMaster.Models;

namespace StockMaster.Mapping
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, ContactDto>()
                .ForMember(dest => dest.Orders, opt => opt.MapFrom(src => src.PurchaseOrders));

            CreateMap<ContactDto, Supplier>();

            CreateMap<CreateContactDto, Supplier>();

            CreateMap<Supplier, ContactSimpleDto>();
        }
    }
}
