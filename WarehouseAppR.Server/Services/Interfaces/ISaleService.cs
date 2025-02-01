using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface ISaleService
    {
        public Task<IEnumerable<SaleDTO>> GetAllSalesFull();
        public Task<IEnumerable<SaleDTO>> GetSalesByUser(Guid id);
        public Task<IEnumerable<SaleDTO>> SearchSalesByUser(Guid id, string ean, string series, DateOnly? dateFrom, DateOnly? dateTo);
        public Task<IEnumerable<SaleDTO>> SearchSalesFull(string fullName, string ean, string series, DateOnly? dateFrom, DateOnly? dateTo);
        //Add SalesByUser and ByDate
    }
}
