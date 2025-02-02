using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IAdminService
    {
        public void SetCurrency(string currency);
        public CurrencyDTO GetCurrency();
        public Task DeleteBySeries(string series, bool stockDelivery, bool sales);
        public Task DeleteByEan(string ean);
    }
}
