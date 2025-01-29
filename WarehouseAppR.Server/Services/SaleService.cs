using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Services
{
    public class SaleService : ISaleService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;
        public SaleService(WarehouseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SaleDTO>> GetAllSales()
        {
            var sales = await _dbContext.Sales.ToListAsync();
            var salesDtos = _mapper.Map<List<SaleDTO>>(sales);
            return salesDtos;
        }
        public async Task<IEnumerable<SaleDTO>> SearchSalesExtedned(string fullName, string ean, string series, DateOnly? dateFrom, DateOnly? dateTo)
        {
            var query = _dbContext.Sales.AsQueryable();
            if (fullName.Length != 0)
                query = query.Where(s => (s.User.FirstName + " " + s.User.LastName).ToLower() == fullName.ToLower());
            if (ean.Length != 0)
                query = query.Where(s => s.Product.Ean == ean);
            if (series.Length != 0)
                query = query.Where(s => s.Series == series);
            if (dateFrom.HasValue)
                query = query.Where(s => s.DateSaled >= dateFrom.Value);
            if (dateTo.HasValue)
                query = query.Where(s => s.DateSaled <= dateTo.Value);
            return _mapper.Map<List<SaleDTO>>(await query.ToListAsync());
        }

        public async Task<IEnumerable<SaleDTO>> GetSalesByUser(Guid id)
        {
            var sales = await _dbContext.Sales.Where(s => s.UserId == id).ToListAsync();
            var salesDto = _mapper.Map<List<SaleDTO>>(sales);
            return salesDto;
        }
        public async Task<IEnumerable<SaleDTO>> SearchSalesByUser(Guid id, string ean, string series, DateOnly? dateFrom, DateOnly? dateTo)
        {
            var query = _dbContext.Sales.Where(s => s.UserId == id);
            if (ean.Length != 0)
                query = query.Where(s => s.Product.Ean == ean);
            if (series.Length != 0)
                query = query.Where(s => s.Series == series);
            if (dateFrom.HasValue)
                query = query.Where(s => s.DateSaled >= dateFrom.Value);
            if (dateTo.HasValue)
                query = query.Where(s => s.DateSaled <= dateTo.Value);
            return _mapper.Map<List<SaleDTO>>(await query.ToListAsync());
        }
    }
}
