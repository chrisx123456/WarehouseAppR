using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WarehouseAppR.Server.Models
{
    public class Stock
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        [Key]
        public Guid StockId { get; set; }
        [Required]
        [ForeignKey(nameof(WarehouseAppR.Server.Models.Product))]
        public Guid ProductId{ get; set; }
        public virtual Product? Product { get; set; }
        [Required]
        public required string Series {  get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public required DateOnly? ExpirationDate { get; set; }
        [Required]
        public required string StorageLocationCode { get; set; }

    }
}
