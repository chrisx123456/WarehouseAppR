using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class Sale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        [Key]
        public Guid SaleId { get; set; }
        [Required]
        [ForeignKey(nameof(Database.User))]
        public required Guid UserId { get; set; }
        public virtual User? User { get; set; } 
        [Required]
        [ForeignKey(nameof(Database.Product))]
        public required Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required decimal Price { get; set; }
        [Required]
        public required decimal Profit { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public required DateOnly DateSaled { get; set; }
        [Required]
        public required string Series { get; set; }

    }
}
