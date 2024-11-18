﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseAppR.Server.Models
{
    public class Manufacturer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DataType(nameof(Guid))]
        public Guid ManufacturerId { get; set; }
        [Required]
        public required string Name { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
