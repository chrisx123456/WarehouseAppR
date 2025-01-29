using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class CurrencyDTO
    {
        [Required]
        public required string Currency { get; set; }
    }
}
