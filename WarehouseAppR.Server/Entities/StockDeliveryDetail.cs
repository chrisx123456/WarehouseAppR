using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models
{
    public class StockDeliveryDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        [Key]
        public Guid StockDeliveryDetailId { get; set; }
        [Required]
        [ForeignKey(nameof(WarehouseAppR.Server.Models.StockDelivery))]
        public Guid StockDeliveryId { get; set; }
        public virtual StockDelivery StockDelivery { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateDelivered { get; set; }
        [Required]
        public int AcceptorId { get; set; }
    }
}
