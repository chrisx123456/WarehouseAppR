﻿using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.Models
{
    public class StockDTO
    {
        [Required]
        public required string Name {  get; set; }
        [Required]
        public required string Series { get; set; }
        [Required]
        public required decimal Quantity { get; set; }
        public DateOnly? ExpDate { get; set; }
        [Required]
        public required string StorageLocationCode { get; set; }
    }
}
