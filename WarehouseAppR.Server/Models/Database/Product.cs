﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WarehouseAppR.Server.DataAnnotations;

namespace WarehouseAppR.Server.Models.Database
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        public Guid ProductId { get; set; }
        [Required]
        [ForeignKey(nameof(Database.Manufacturer))]
        public required Guid ManufacturerId { get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }  // Nawigacja do Manufacturer
        [Required]

        [ForeignKey(nameof(Database.Category))]
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
        [Ean]
        public required string Ean { get; set; }
        public string? Description { get; set; }

        public virtual List<StockDelivery>? StockDeliveries { get; set; }
        public virtual List<Stock>? InStock { get; set; }


    }
}
