using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseAppR.Server.Models;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Services
{
    public class StockService : IStockService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;
        public StockService(WarehouseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<IEnumerable<StockDTO>> GetAllInStock()
        {
            var allInStock = await _dbContext.InStock.ToListAsync();
            var allInStockDtos = _mapper.Map<List<StockDTO>>(allInStock);
            return allInStockDtos;

        }

        public async Task<IEnumerable<StockDTO>> GetInStockByEan(string ean)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Ean.Equals(ean));
            var inStock = product?.InStock;
            var inStockDtos = _mapper.Map<List<StockDTO>>(inStock);
            return inStockDtos;
        }

        public async Task<StockDTO> GetInStockBySeries(string series)
        {
            var inStock = await _dbContext.InStock.SingleOrDefaultAsync(s => s.Series.ToLower().Equals(series.ToLower()));
            var inStockDto = _mapper.Map<StockDTO>(inStock);
            return inStockDto;
        }

        public async Task<IEnumerable<StockDTO>> GetInStockFromDate(DateOnly date)
        {
            var inStock = await _dbContext.InStock.Where(s => s.ExpirationDate >= date).ToListAsync();
            var inStockDtos = _mapper.Map<List<StockDTO>>(inStock);
            return inStockDtos;
        }
    }
}
