using System.ComponentModel.DataAnnotations;

namespace WarehouseAppR.Server.DTOs
{
    public class CategoryDTO
    {
        public required string Name { get; set; }
        public int Vat { get; set; }
    }
}
