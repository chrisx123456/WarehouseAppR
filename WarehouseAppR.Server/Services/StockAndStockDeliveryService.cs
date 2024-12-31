using AutoMapper;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Services.Interfaces;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;
using System.Security.Claims;
namespace WarehouseAppR.Server.Services
{
    public class StockAndStockDeliveryService : IStockAndStockDeliveryService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper; 
        private readonly IHttpContextAccessor _contextAccessor;
        public StockAndStockDeliveryService(WarehouseDbContext dbContext, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }
        public async Task AddDelivery(AddNewStockDeliveryDTO newStockDeliveryDTO)
        {
            var isStockSeriesExists = _dbContext.InStock.SingleOrDefault(s => s.Series.Equals(newStockDeliveryDTO.Series));
            var isStockDeliverySeriesExists = _dbContext.StockDeliveries.SingleOrDefault(sd => sd.Series == newStockDeliveryDTO.Series);
            if(isStockSeriesExists != null || isStockDeliverySeriesExists != null)
            {
                throw new ItemAlreadyExistsException("Item with such series already exsits in InStock table or StockDeliveries");
            }
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            Stock stock = _mapper.Map<Stock>(newStockDeliveryDTO);
            StockDelivery stockDelivery = _mapper.Map<StockDelivery>(newStockDeliveryDTO, opts => opts.Items["UserId"] = userId);
            await _dbContext.StockDeliveries.AddAsync(stockDelivery);
            await _dbContext.InStock.AddAsync(stock);
            await _dbContext.SaveChangesAsync();
        }
    }
}
