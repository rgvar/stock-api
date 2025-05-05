using AutoMapper;
using StockMaster.Dtos;
using StockMaster.Models;

namespace StockMaster.Mapping
{
    public class PurchaseOrderProfile : Profile
    {
        public PurchaseOrderProfile()
        {
            CreateMap<OrderDto, PurchaseOrder>();

            CreateMap<PurchaseOrder, OrderDto>()
                .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Supplier));

            CreateMap<CreateOrderDto, PurchaseOrder>()
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<PurchaseOrder, OrderForInvoiceDto>();

            CreateMap<PurchaseOrder, OrderForContactDto>();

        }
    }
}
