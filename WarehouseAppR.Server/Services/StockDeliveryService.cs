using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services
{
    public class StockDeliveryService : IStockDeliveryService
    {
        private readonly WarehouseDbContext _dbContext;
        public StockDeliveryService(WarehouseDbContext dbContext)
        {
             _dbContext = dbContext;
        }

        public Task AddDelivery(StockDeliveryDTO delivery)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockDeliveryDTO>> GetAllDeliveries()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockDeliveryDTO>> GetDeliveriesByDate(DateOnly date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockDeliveryDTO>> GetDeliveriesByEan(string ean)
        {
            throw new NotImplementedException();
        }
    }
}
