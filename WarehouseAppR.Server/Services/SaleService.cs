using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    }
}
