using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class Stock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        [Key]
        public Guid StockId { get; set; }
        [Required]
        [ForeignKey(nameof(Database.Product))]
        public required Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        [Required]
        public required string Series { get; set; }
        public virtual StockDelivery StockDelivery { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [DataType(DataType.Date)]
        public required DateOnly? ExpirationDate { get; set; }
        [Required]
        public required string StorageLocationCode { get; set; }

    }
}
