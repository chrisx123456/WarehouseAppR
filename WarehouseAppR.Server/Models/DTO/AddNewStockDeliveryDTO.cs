using System.ComponentModel.DataAnnotations;
using WarehouseAppR.Server.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class AddNewStockDeliveryDTO
    {
        [Required]
        [Ean]
        public required string Ean { get; set; }
        [Required]
        public required string Series { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required decimal PricePaid { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        [Required]
        public required string StorageLocationCode { get; set; }
    }
}
