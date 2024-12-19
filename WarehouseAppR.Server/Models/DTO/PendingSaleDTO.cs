using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class PendingSaleDTO
    {
        [Required]
        public required Guid PreviewId { get; set; }
        [Required]
        public required IEnumerable<SaleDTO> PendingSales {  get; set; }
    }
}
