using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class SaleList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        public Guid SaleListId { get; set; }
        [Required]
        public required Guid ProductSaleId { get; set; }
        [Required]
        public required string Ean { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required string Series { get; set; }

    }
}
