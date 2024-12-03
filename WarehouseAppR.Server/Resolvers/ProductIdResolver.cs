using AutoMapper;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Resolvers
{
    public class ProductIdResolver : IValueResolver<AddNewStockDeliveryDTO, StockDelivery, Guid>,
                                     IValueResolver<AddNewStockDeliveryDTO, Stock, Guid>
    {
        private readonly WarehouseDbContext _dbContext;
        public ProductIdResolver(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Resolve(AddNewStockDeliveryDTO source, StockDelivery destination, Guid destMember, ResolutionContext context)
        {
            return _dbContext.Products.FirstOrDefault(p => p.Ean == source.Ean)?.ProductId
                ?? throw new NotFoundException("No such product found while mapping");
        }
        public Guid Resolve(AddNewStockDeliveryDTO source, Stock destination, Guid destMember, ResolutionContext context)
        {
            return _dbContext.Products.FirstOrDefault(p => p.Ean == source.Ean)?.ProductId
                ?? throw new NotFoundException("No such product found while mapping");
        }

    }
}
