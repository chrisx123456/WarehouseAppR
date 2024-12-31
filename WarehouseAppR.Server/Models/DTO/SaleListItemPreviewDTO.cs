using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class SaleListItemPreviewDTO
    {
        [Required]
        public required string ProductName { get; set; }
        [Required]
        public required string TradeName { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required decimal Price { get; set; }
        [Required]
        public required string Series { get; set; }
        [Required]
        public required string Ean { get; set; }

    }
}
