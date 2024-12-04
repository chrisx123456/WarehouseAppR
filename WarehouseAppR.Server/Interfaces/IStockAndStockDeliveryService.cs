using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Interfaces
{
    public interface IStockAndStockDeliveryService
    {
        public Task AddDelivery(AddNewStockDeliveryDTO newStockDeliveryDTO);
    }
}
