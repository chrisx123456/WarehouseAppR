using AutoMapper;
using NLog.LayoutRenderers;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services
{
    public class ProductService : IProductService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;
        public ProductService(WarehouseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void AddNewProduct(ProductDTO productDto)
        {
            var product = GetByEan(productDto.Ean);
            if (product is not null) throw new ItemAlreadyExistsException($"Product with ean {productDto.Ean} exists");
            var productToAdd = _mapper.Map<Product>(productDto);
            _dbContext.Products.Add(productToAdd);
            _dbContext.SaveChanges();
        }

        public void DeleteProductByEan(string ean)
        {
            var product = GetByEan(ean);
            if (product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ProductDTO> GetAllProducts()
        {
            var products = _dbContext.Products.ToList();
            var productsDto = _mapper.Map<List<ProductDTO>>(products);
            return productsDto;
        }

        public ProductDTO GetProductByEan(string ean)
        {
            var product = GetByEan(ean);
            if (product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
            var productDto = _mapper.Map<ProductDTO>(product);
            return productDto;
        }
        
        public IEnumerable<ProductDTO> GetProductsByName(string name)
        {
            var products = _dbContext.Products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
            var productsDtos = _mapper.Map<List<ProductDTO>>(products);
            return productsDtos;
        }

        public IEnumerable<ProductDTO> GetProductsByTradeName(string tradeName)
        {
            var products = _dbContext.Products.Where(p => p.TradeName.ToLower().Contains(tradeName.ToLower())).ToList();
            var productsDtos = _mapper.Map<List<ProductDTO>>(products);
            return productsDtos;
        }

        public void UpdateDescription(string ean, string? description)
        {
            var product = GetByEan(ean);
            if (product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
            product.Description = description ?? "";
            _dbContext.SaveChanges();
        }

        public void UpdateProductPrice(string ean, decimal newPrice)
        {
            var product = GetByEan(ean);
            if(product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
            product.Price = newPrice;
            _dbContext.SaveChanges();
        }
        private Product? GetByEan(string ean) 
        {
            return _dbContext.Products.FirstOrDefault(p => p.Ean.ToLower() == ean.ToLower());
        }
    }
}
