using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class SaleDTO
    {
        [Required]
        public required string TradeName {  get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required decimal AmountPaid { get; set; }
        [Required]
        public required decimal Profit { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public required DateOnly DateSaled { get; set; }
        [Required]
        public required string Series { get; set; }
        [Required]
        public required string UserFullName { get; set; }
        [Required]
        [Ean]
        public required string Ean {  get; set; }
    }
}
