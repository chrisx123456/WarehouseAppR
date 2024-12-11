using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IStockAndStockDeliveryService
    {
        public Task AddDelivery(AddNewStockDeliveryDTO newStockDeliveryDTO);
    }
}
