using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        public Guid CategoryId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required int Vat { get; set; }
    }
}
