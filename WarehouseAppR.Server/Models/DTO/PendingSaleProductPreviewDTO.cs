using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class PendingSaleProductPreviewDTO
    {
        [Required]
        public required string Ean { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string TradeName { get; set; }
        [Required]
        public required string Series { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required decimal AmountToBePaid { get; set; }
        [Required]
        public required decimal Profit { get; set; }
    }
}
