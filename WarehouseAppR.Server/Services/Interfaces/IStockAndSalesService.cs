using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IStockAndSalesService
    {
        public Task<IEnumerable<Sale>> Sell(string ean, decimal count);
    }
}
