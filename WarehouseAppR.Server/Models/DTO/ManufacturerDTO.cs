using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class ManufacturerDTO
    {
        [Required]
        public required string Name { get; set; }
    }
}
