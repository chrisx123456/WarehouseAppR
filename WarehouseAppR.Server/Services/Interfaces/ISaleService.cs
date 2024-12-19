using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface ISaleService
    {
        public Task<IEnumerable<SaleDTO>> GetAllSales();
        //Add SalesByUser and ByDate
    }
}
