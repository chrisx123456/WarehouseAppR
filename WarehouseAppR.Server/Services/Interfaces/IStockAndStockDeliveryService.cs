using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IStockAndStockDeliveryService
    {
        public Task AddDelivery(AddNewStockDeliveryDTO newStockDeliveryDTO);
    }
}
