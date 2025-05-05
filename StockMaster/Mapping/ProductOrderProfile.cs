using AutoMapper;
using StockMaster.Dtos;
using StockMaster.Entities;

namespace StockMaster.Mapping
{
    public class ProductOrderProfile : Profile
    {
        public ProductOrderProfile()
        {
            CreateMap<ProductOrder, ProductOrderDto>();
        }
    }
}
