using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.DTOs
{
    public class ManufacturerDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}
