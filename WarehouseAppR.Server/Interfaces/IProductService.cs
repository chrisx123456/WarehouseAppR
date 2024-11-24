using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Interfaces
{
    public interface IProductService
    {
        public IEnumerable<ProductDTO> GetAllProducts();
        public IEnumerable<ProductDTO> GetProductByName(string name);
        public IEnumerable<ProductDTO> GetProductByTradeName(string tradeName);
        public IEnumerable<ProductDTO> GetProductByEan(string ean);
        public void UpdateProductPrice(string ean, decimal newPrice);

        public void DeleteProductByEan(string ean);
        public void AddNewProduct(ProductDTO category);
    }
}
