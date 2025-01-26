using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class PendingSale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ForeignKey(nameof(Database.PendingSaleProduct))]
        [DataType(nameof(Guid))]
        public Guid PendingSaleId { get; set; }
        public virtual List<PendingSaleProduct> PendingSaleProducts { get; set; }
        [Required]
        public required DateOnly DateAdded { get; set; }
    }
}
