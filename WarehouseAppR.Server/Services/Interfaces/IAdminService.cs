using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IAdminService
    {
        public void SetCurrency(string currency);
        public CurrencyDTO GetCurrency();
    }
}
