using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class PasswordDTO
    {
        [Required]
        public required string Password { get; set; }
    }
}
