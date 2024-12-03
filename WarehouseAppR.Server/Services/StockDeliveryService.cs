using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services
{
    public class StockDeliveryService : IStockDeliveryService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;
        public StockDeliveryService(WarehouseDbContext dbContext, IMapper mapper)
        {
             _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task AddDelivery(AddNewStockDeliveryDTO newdelivery)
        {
            var delivery = await _dbContext.StockDeliveries.SingleOrDefaultAsync(d => d.Series == newdelivery.Series);
            if (delivery is not null) throw new ItemAlreadyExistsException("Delivery with such series was accepted before");
            Stock stock = _mapper.Map<Stock>(newdelivery);
            StockDelivery stockDelivery = _mapper.Map<StockDelivery>(newdelivery);
            await _dbContext.InStock.AddAsync(stock);
            await _dbContext.StockDeliveries.AddAsync(stockDelivery);
            await _dbContext.SaveChangesAsync();
            //Stock and StockDel. are pretty much same tables, but StockDel is mostly for info purpose.
            //Adding both stockdelivery and stock because when you accept delivery it means it is in-stock.
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
