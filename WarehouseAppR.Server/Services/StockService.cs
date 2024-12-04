using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

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
        public Task DecreaseInStockQuantity(string ean, decimal count)
        {
            

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<StockDTO>> GetAllInStock()
        {
            //var allInStock = await _dbContext.InStock.ToListAsync();
            //var allInStock
            //return allInStock;
            throw new NotImplementedException();

        }

        public async Task<IEnumerable<StockDTO>> GetInStockByEan(string ean)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Ean.Equals(ean));
            var inStock = product?.Available;
            throw new NotImplementedException();
        }

        public Task<StockDTO> GetInStockBySeries(string series)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StockDTO>> GetInStockFromDate(DateOnly date)
        {
            throw new NotImplementedException();
        }
    }
}
