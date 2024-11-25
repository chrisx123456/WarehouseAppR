using AutoMapper;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Models;
using WarehouseAppR.Server.Resolvers;

namespace WarehouseAppR.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<ManufacturerDTO, Manufacturer>();
            CreateMap<Manufacturer, ManufacturerDTO>();

            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ManufacturerName, c => c.MapFrom(src => src.Manufacturer.Name))
                .ForMember(dest => dest.Name, c => c.MapFrom(src => src.Name))
                .ForMember(dest => dest.TradeName, c => c.MapFrom(src => src.TradeName))
                .ForMember(dest => dest.CategoryName, c => c.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.UnitType, c => c.MapFrom(src => src.UnitType))
                .ForMember(dest => dest.Price, c => c.MapFrom(src => src.Price))
                .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Ean))
                .ForMember(dest => dest.Description, c => c.MapFrom(src => src.Description));

            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.ManufacturerId, c => c.MapFrom<ManufacturerIdResolver>())
                .ForMember(dest => dest.CategoryId, c => c.MapFrom<CategoryIdResolver>())
                .ForMember(dest => dest.Name, c => c.MapFrom(src => src.Name))
                .ForMember(dest => dest.TradeName, c => c.MapFrom(src => src.TradeName))
                .ForMember(dest => dest.UnitType, c => c.MapFrom(src => src.UnitType))
                .ForMember(dest => dest.Price, c => c.MapFrom(src => src.Price))
                .ForMember(dest => dest.Ean, c => c.MapFrom(src => src.Ean))
                .ForMember(dest => dest.Description, c => c.MapFrom(src => src.Description));
                
        }
    }
}
