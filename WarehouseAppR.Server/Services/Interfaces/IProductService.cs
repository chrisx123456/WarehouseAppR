using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDTO>> GetAllProducts();
        public Task<IEnumerable<ProductDTO>> GetProductsByName(string name);
        public Task<IEnumerable<ProductDTO>> GetProductsByTradeName(string tradeName);
        public Task<ProductDTO> GetProductByEan(string ean);
        public Task UpdateProductPrice(string ean, decimal newPrice);

        public Task DeleteProductByEan(string ean);
        public Task AddNewProduct(ProductDTO category);
        public Task UpdateDescription(string ean, string? description);
    }
}
