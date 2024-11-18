using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models
{
    public class Sale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        [Key]
        public Guid SaleId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [ForeignKey(nameof(WarehouseAppR.Server.Models.Product))]
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; }

    }
}
