using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models
{
    public class StockDelivery
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        [Key]
        public Guid StockDeliveryId { get; set; }
        [Required]
        [ForeignKey(nameof(WarehouseAppR.Server.Models.Product))]
        public required Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        [Required]
        public required string Series { get; set; }
        [Required]
        public required int Quantity { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public required DateOnly DateDelivered { get; set; }
        [Required]
        public required int AcceptorId { get; set; }
    }
}
