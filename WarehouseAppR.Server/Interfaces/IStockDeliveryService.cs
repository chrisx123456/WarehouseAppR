using Microsoft.EntityFrameworkCore.Proxies.Internal;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Interfaces
{
    public interface IStockDeliveryService
    {
        public IEnumerable<StockDeliveryDTO> GetAllDeliveries();
        public IEnumerable<StockDeliveryDTO> GetDeliveriesByEan(string ean);
        public IEnumerable<StockDeliveryDTO> GetDeliveriesByDate(DateOnly date);
        ///public IEnumerable<StockDeliveryDTO> GetDeliveriesByAcceptor(DateOnly date);

        public void AddDelivery(StockDeliveryDTO delivery);
        //Todo:
        // + Admin options: edit entities
        // + DeliveryDTO w. guid

    }
}
