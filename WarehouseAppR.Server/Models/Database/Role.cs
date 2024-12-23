using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        public Guid RoleId { get; set; }
        [Required]
        public required string RoleName { get; set; }

    }
}
