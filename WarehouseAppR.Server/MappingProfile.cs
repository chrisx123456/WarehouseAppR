using AutoMapper;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryDTO, Category>();
            CreateMap<Category, CategoryDTO>();
        }
    }
}
