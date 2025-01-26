using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface ISellingProductsService
    {
        public Task<PendingSalePreviewDTO> GeneratePendingSalePreview(List<NewProductSaleDTO> productsToSale);
        public Task ConfirmSale(Guid pendingSaleId);
        public Task RejectSale(Guid pendingSaleId);
    }
}
