using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class LoginDTO
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }

    }
}
