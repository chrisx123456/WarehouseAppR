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
        [ForeignKey(nameof(Database.SaleList))]
        public required Guid ProductSaleId { get; set; }
        public virtual List<SaleList>? SaleLists { get; set; }
        [Required]
        public required DateOnly DateAdded { get; set; }
    }
}
