using AutoMapper;
using StockMaster.Dtos;
using StockMaster.Models;

namespace StockMaster.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories));

            CreateMap<ProductDto, Product>();

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Categories, opt => opt.Ignore())
                .ForMember(dest => dest.StockQuantity, opt => opt.Ignore());

            CreateMap<CreateProductDto, Product>();
            
        }
    }
}
