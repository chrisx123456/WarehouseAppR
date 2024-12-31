using System.ComponentModel.DataAnnotations;
using WarehouseAppR.Server.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class ProductSaleDTO
    {
        [Required]
        [Ean]
        public required string Ean { get; set; }
        [Required]
        public required decimal Count { get; set; }
    }
}
