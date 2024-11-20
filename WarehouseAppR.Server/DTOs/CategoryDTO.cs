using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.DTOs
{
    public class CategoryDTO
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        [Range(0, 99)]
        public int Vat { get; set; }
    }
}
