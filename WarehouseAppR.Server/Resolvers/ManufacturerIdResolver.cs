using AutoMapper;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Resolvers
{
    public class ManufacturerIdResolver : IValueResolver<ProductDTO, Product, Guid>
    {
        private readonly WarehouseDbContext _dbContext;
        public ManufacturerIdResolver(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Guid Resolve(ProductDTO source, Product destination, Guid destMember, ResolutionContext context)
        {
            return _dbContext.Manufacturers.FirstOrDefault(m => string.Equals(m.Name, source.Name, StringComparison.OrdinalIgnoreCase))?.ManufacturerId
                  ?? throw new NotFoundException("No such manufacturer found while mapping");
            //return _dbContext.Manufacturers.FirstOrDefault(m => m.Name.ToLower() == source.ManufacturerName.ToLower())?.ManufacturerId 
            //    ?? throw new NotFoundException("No such manufacturer found while mapping");
        }
    }
}
