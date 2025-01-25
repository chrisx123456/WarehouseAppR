using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WarehouseAppR.Server.Models.Database;

namespace WarehouseAppR.Server.Models.DTO
{
    public class SaleDTO
    {
        [Required]
        public required Guid ProductId { get; set; }
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
        [Required]
        public required int UserId { get; set; }
    }
}
