using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseAppR.Server.Models.Database
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        public Guid UserId { get; set; }
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        [ForeignKey(nameof(Database.Role))]
        public required Guid RoleId { get; set; }
        public virtual Role? Role { get; set; } 
    }
}
