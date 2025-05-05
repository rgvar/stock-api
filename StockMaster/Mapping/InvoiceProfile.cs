using AutoMapper;
using StockMaster.Dtos;
using StockMaster.Entities;
using StockMaster.Models;

namespace StockMaster.Mapping
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Invoice, InvoiceDto>();

            CreateMap<InvoiceDto, Invoice>();

            CreateMap<CreateInvoiceDto, Invoice>();

            CreateMap<Invoice, InvoiceSimpleDto>();

            CreateMap<Order, OrderForInvoiceDto>()
                .Include<SalesOrder, OrderForInvoiceDto>()
                .Include<PurchaseOrder, OrderForInvoiceDto>();

            CreateMap<InvoiceData, CreateInvoiceDto>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore());

        }
    }
}
