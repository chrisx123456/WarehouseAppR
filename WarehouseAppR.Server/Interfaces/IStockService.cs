using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Interfaces
{
    public interface IStockService
    {
        public Task<IEnumerable<StockDTO>> GetAllInStock();
        public Task<IEnumerable<StockDTO>> GetInStockByEan(string ean);
        public Task<IEnumerable<StockDTO>> GetInStockFromDate(DateOnly date);
        public Task<StockDTO> GetInStockBySeries(string series);
        public Task DecreaseInStockQuantity(string ean, decimal count);
        //Dodac jakis DTO ktory bedzie sumował te same produkty z rozszerzonym widokiem na products


    }
}
