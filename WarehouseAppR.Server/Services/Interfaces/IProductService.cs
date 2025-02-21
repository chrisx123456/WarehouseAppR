using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDTO>> GetAllProducts();
        public Task<IEnumerable<ProductDTO>> GetProductsByName(string name);
        public Task<ProductDTO> GetProductByTradeName(string tradeName);
        public Task<ProductDTO> GetProductByEan(string ean);
        public Task DeleteProductByEan(string ean);
        public Task AddNewProduct(ProductDTO product);
        public Task UpdateProduct(ProductPatchDTO patchData, string ean);
    }
}
