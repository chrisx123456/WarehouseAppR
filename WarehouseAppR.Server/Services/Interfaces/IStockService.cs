using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IStockService
    {
        public Task<IEnumerable<StockDTO>> GetAllInStock();
        public Task<IEnumerable<StockDTO>> GetInStockByEan(string ean);
        public Task<IEnumerable<StockDTO>> GetInStockFromDate(DateOnly date);
        public Task<StockDTO> GetInStockBySeries(string series);
        //public Task Sell(string ean, decimal count); //To gdzie indziej
        //Dodac jakis DTO ktory bedzie sumował te same produkty z rozszerzonym widokiem na products


    }
}
