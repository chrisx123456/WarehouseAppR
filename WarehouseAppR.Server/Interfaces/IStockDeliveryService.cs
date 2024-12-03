using Microsoft.EntityFrameworkCore.Proxies.Internal;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Interfaces
{
    public interface IStockDeliveryService
    {
        public Task<IEnumerable<StockDeliveryDTO>> GetAllDeliveries();
        public Task<IEnumerable<StockDeliveryDTO>> GetDeliveriesByEan(string ean);
        public Task<IEnumerable<StockDeliveryDTO>> GetDeliveriesByDate(DateOnly date);
        ///public IEnumerable<StockDeliveryDTO> GetDeliveriesByAcceptor(DateOnly date);

        public Task AddDelivery(StockDeliveryDTO delivery);
        //Todo:
        // + Admin options: edit entities
        // + DeliveryDTO w. guid

    }
}
