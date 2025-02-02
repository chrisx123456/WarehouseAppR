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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StockAndStockDeliveryService(WarehouseDbContext dbContext, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = contextAccessor;
        }
        public async Task AddDelivery(AddNewStockDeliveryDTO newStockDeliveryDTO)
        {
            if(newStockDeliveryDTO.Quantity <= 0 || newStockDeliveryDTO.PricePaid <= 0)
                throw new ForbiddenActionPerformedException("Price paid and Quantity must be greater than 0");

            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new LoginException("You need to log in again");

            string? idString = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idString == null)
                throw new LoginException("you need to log in again");

            Guid userId = Guid.Parse(idString);

            var isStockSeriesExists = _dbContext.InStock.SingleOrDefault(s => s.Series.Equals(newStockDeliveryDTO.Series));
            var isStockDeliverySeriesExists = _dbContext.StockDeliveries.SingleOrDefault(sd => sd.Series == newStockDeliveryDTO.Series);
            if(isStockSeriesExists != null || isStockDeliverySeriesExists != null)
            {
                throw new ItemAlreadyExistsException("Item with such series already exsits in InStock table or StockDeliveries");
            }


            Stock stock = _mapper.Map<Stock>(newStockDeliveryDTO);
            StockDelivery stockDelivery = _mapper.Map<StockDelivery>(newStockDeliveryDTO, opts => opts.Items["UserId"] = userId);
            await _dbContext.StockDeliveries.AddAsync(stockDelivery);
            await _dbContext.InStock.AddAsync(stock);
            await _dbContext.SaveChangesAsync();
        }
    }
}
