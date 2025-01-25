using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class UserSaleDTO
    {
        [Required]
        public required string TradeName { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required decimal Price { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public required DateOnly DateSaled { get; set; }
        [Required]
        public required string Series { get; set; }
    }
}
