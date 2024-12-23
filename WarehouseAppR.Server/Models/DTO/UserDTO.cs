using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class UserDTO
    {
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string RoleName { get; set; }
    }
}
