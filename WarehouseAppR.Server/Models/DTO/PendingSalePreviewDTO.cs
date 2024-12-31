using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class PendingSalePreviewDTO
    {
        [Required]
        public required Guid PreviewId { get; set; }
        [Required]
        public required IEnumerable<SaleListItemPreviewDTO> PendingSales { get; set; }
    }
}
