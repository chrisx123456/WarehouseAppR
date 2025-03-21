﻿using System.ComponentModel.DataAnnotations;
using WarehouseAppR.Server.DataAnnotations;

namespace WarehouseAppR.Server.Models.DTO
{
    public class ProductDTO
    {
        [Required]
        public required string ManufacturerName { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string TradeName { get; set; }
        [Required]
        public required string CategoryName { get; set; }
        [Required]
        public required Units UnitType { get; set; }
        [Required]
        public required decimal Price { get; set; }
        [Required]
        [Ean]
        public required string Ean { get; set; }
        public string? Description { get; set; }
    }
}
