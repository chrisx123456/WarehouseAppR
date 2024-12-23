using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Services
{
    public class StockDeliveryService : IStockDeliveryService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;
        public StockDeliveryService(WarehouseDbContext dbContext, IMapper mapper, IStockAndStockDeliveryService stockAndStockDeliveryService)
        {
             _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<StockDeliveryDTO>> GetAllDeliveries()
        {
            var deliveries = await _dbContext.StockDeliveries.ToListAsync();
            var deliveriesDtos = _mapper.Map<List<StockDeliveryDTO>>(deliveries);
            return deliveriesDtos;
        }

        public async Task<IEnumerable<StockDeliveryDTO>> GetDeliveriesByDate(DateOnly date)
        {
            var deliveries = await _dbContext.StockDeliveries.Where(s => s.DateDelivered.Equals(date)).ToListAsync();
            var deliveriesDtos = _mapper.Map<List<StockDeliveryDTO>>(deliveries);
            return deliveriesDtos;
        }

        public async Task<IEnumerable<StockDeliveryDTO>> GetDeliveriesByEan(string ean)
        {
            var deliveries = await _dbContext.StockDeliveries.Where(s => s.Product.Ean == ean).ToListAsync();
            var deliveriesDtos = _mapper.Map<List<StockDeliveryDTO>>(deliveries);
            return deliveriesDtos;
        }
    }
}
