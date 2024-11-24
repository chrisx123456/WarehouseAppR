using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services
{
    public class ProductService : IProductService
    {
        public void AddNewProduct(ProductDTO category)
        {
            throw new NotImplementedException();
        }

        public void DeleteProductByEan(string ean)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductDTO> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductDTO> GetProductByEan(string ean)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductDTO> GetProductByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductDTO> GetProductByTradeName(string tradeName)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductPrice(string ean, decimal newPrice)
        {
            throw new NotImplementedException();
        }
    }
}
