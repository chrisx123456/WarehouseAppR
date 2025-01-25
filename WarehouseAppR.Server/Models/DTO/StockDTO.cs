using System.ComponentModel.DataAnnotations;
using WarehouseAppR.Server.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class StockDTO
    {
        [Required]
        public required string TradeName { get; set; }
        [Ean]
        [Required]
        public required string Ean { get; set; }
        [Required]
        public required string Series { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        [Required]
        public required string StorageLocationCode { get; set; }
        [Required]
        public required Units UnitType { get; set; }
        [Required]
        public required decimal PricePaid { get; set; }
    }
}
