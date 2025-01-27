using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class StockDelivery
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        [Key]
        public Guid StockDeliveryId { get; set; }
        [Required]
        [ForeignKey(nameof(Database.Product))]
        public required Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public required string Series { get; set; }
        public virtual Stock? Stock { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public required DateOnly DateDelivered { get; set; }
        [Required]
        public required decimal PricePaid { get; set; }
        [Required]
        [ForeignKey(nameof(Database.User))]
        public required Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
