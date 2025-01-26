using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class PendingSalePreviewDTO
    {
        [Required]
        public required Guid PendingSaleId { get; set; }
        [Required]
        public required List<PendingSaleProductPreviewDTO> ProductPreviews { get; set; }
    }
}
