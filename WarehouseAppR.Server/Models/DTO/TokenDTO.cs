using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class TokenDTO
    {
        [Required]
        public required string Token { get; set; }
    }
}
