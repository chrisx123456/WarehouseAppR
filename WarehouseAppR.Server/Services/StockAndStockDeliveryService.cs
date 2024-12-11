using AutoMapper;
using WarehouseAppR.Server.Models;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Services.Interfaces;
namespace WarehouseAppR.Server.Services
{
    public class StockAndStockDeliveryService : IStockAndStockDeliveryService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper; 
        public StockAndStockDeliveryService(WarehouseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task AddDelivery(AddNewStockDeliveryDTO newStockDeliveryDTO)
        {
            var isStockSeriesExists = _dbContext.InStock.SingleOrDefault(s => s.Series.Equals(newStockDeliveryDTO.Series));
            var isStockDeliverySeriesExists = _dbContext.StockDeliveries.SingleOrDefault(sd => sd.Series == newStockDeliveryDTO.Series);
            if(isStockSeriesExists != null || isStockDeliverySeriesExists != null)
            {
                throw new ItemAlreadyExistsException("Item with such series already exsits in InStock table or StockDeliveries");
            }
            Stock stock = _mapper.Map<Stock>(newStockDeliveryDTO);
            StockDelivery stockDelivery = _mapper.Map<StockDelivery>(stock);
            await _dbContext.StockDeliveries.AddAsync(stockDelivery);
            await _dbContext.InStock.AddAsync(stock);
            await _dbContext.SaveChangesAsync();
        }
    }
}
