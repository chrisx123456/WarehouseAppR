using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IStockService
    {
        public Task<IEnumerable<StockDTO>> GetAllInStock();
        public Task<IEnumerable<StockDTO>> GetInStockByEan(string ean);
        public Task<IEnumerable<StockDTO>> GetInStockByDate(DateOnly date);
        public Task<StockDTO> GetInStockBySeries(string series);
    }
}
