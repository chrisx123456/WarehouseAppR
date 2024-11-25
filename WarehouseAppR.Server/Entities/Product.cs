using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseAppR.Server.DataAnnotations;

namespace WarehouseAppR.Server.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        public Guid ProductId { get; set; }
        [Required]
        [ForeignKey(nameof(WarehouseAppR.Server.Models.Manufacturer))]
        public required Guid ManufacturerId { get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }  // Nawigacja do Manufacturer
        [Required]

        [ForeignKey(nameof(WarehouseAppR.Server.Models.Category))]
        public required Guid CategoryId { get; set; }
        public virtual Category? Category { get; set; } // Nawigacja do Category
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string TradeName { get; set; }
        [Required]
        public required Units UnitType { get; set; }
        [Required]
        public required decimal Price { get; set; }
        [Required]
        [StringLength813]
        public required string Ean {  get; set; }
        public string? Description { get; set; }


    }
}
