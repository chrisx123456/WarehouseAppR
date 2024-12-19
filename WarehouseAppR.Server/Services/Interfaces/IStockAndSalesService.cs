using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IStockAndSalesService
    {
        public Task<IEnumerable<SaleDTO>> GenerateSellPreview(string ean, decimal count);
    }
}
