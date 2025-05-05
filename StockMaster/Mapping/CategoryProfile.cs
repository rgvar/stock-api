using AutoMapper;
using StockMaster.Dtos;
using StockMaster.Models;

namespace StockMaster.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategorySimpleDto>();
            CreateMap<CategorySimpleDto, Category>();
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        }
    }
}
