using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface ISaleService
    {
        public Task<IEnumerable<SaleDTO>> GetAllSales();
        public Task<IEnumerable<StockDTO>> GetSalesByUser(Guid id);
        //Add SalesByUser and ByDate
    }
}
