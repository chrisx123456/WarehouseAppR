using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class PendingSaleProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        public Guid PendingSaleProductId { get; set; } //Practically unused
        [Required]
        public required Guid PendingSaleId { get; set; }
        [Required]
        [ForeignKey(nameof(Database.Stock))]
        public required Guid StockId { get; set; }
        public virtual Stock? Stock { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
    }
}
