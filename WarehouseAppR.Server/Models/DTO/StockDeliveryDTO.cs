using System.ComponentModel.DataAnnotations;
using WarehouseAppR.Server.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class StockDeliveryDTO
    {
        [Required]
        [Ean]
        public required string Ean { get; set; }
        [Required]
        public required string Series { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required string TradeName { get; set; }
        [Required]
        public required DateOnly DateDelivered { get; set; }
        [Required]
        public required Guid UserId { get; set; }

    }
}
