using AutoMapper;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Resolvers
{
    public class CategoryIdResolver : IValueResolver<ProductDTO, Product, Guid>
    {
        private readonly WarehouseDbContext _dbContext;
        public CategoryIdResolver(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Resolve(ProductDTO source, Product destination, Guid destMember, ResolutionContext context)
        {
            return _dbContext.Categories.FirstOrDefault(c => c.Name.ToLower() == source.CategoryName.ToLower())?.CategoryId
                ?? throw new NotFoundException("No such category foung while mapping");
        }
    }
}
