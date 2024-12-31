using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class PendingSale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        public Guid PendingSaleId { get; set; }
        [Required]
        public required Guid ProductSaleId { get; set; }
        [Required]
        public required DateOnly DateAdded { get; set; }
    }
}
