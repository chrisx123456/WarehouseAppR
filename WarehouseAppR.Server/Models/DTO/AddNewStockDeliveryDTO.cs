using System.ComponentModel.DataAnnotations;
using WarehouseAppR.Server.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class AddNewStockDeliveryDTO
    {
        [Required]
        [Ean]
        public required string Ean { get; set; }
        [Required]
        public required string Series { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        [Required]
        public required DateOnly DateDelivered { get; set; }
        public DateOnly? ExpDate { get; set; }
        [Required]
        public required int AcceptorId { get; set; }
        [Required]
        public required string StorageLocationCode { get; set; }
    }
}
