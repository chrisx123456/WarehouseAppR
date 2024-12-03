using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        public async Task AddNewProduct(ProductDTO productDto)
        {
            var product = await GetByEan(productDto.Ean);
            if (product is not null) throw new ItemAlreadyExistsException($"Product with ean {productDto.Ean} exists");
            var productToAdd = _mapper.Map<Product>(productDto);
            await _dbContext.Products.AddAsync(productToAdd);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductByEan(string ean)
        {
            var product = await GetByEan(ean);
            if (product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProducts()
        {
            var products = await _dbContext.Products.ToListAsync();
            var productsDto = _mapper.Map<List<ProductDTO>>(products);
            return productsDto;
        }

        public async Task<ProductDTO> GetProductByEan(string ean)
        {
            var product = await GetByEan(ean);
            if (product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
            var productDto = _mapper.Map<ProductDTO>(product);
            return productDto;
        }
        
        public async Task<IEnumerable<ProductDTO>> GetProductsByName(string name)
        {
            var products = await _dbContext.Products.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            var productsDtos = _mapper.Map<List<ProductDTO>>(products);
            return productsDtos;
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByTradeName(string tradeName)
        {
            var products = await _dbContext.Products.Where(p => p.TradeName.ToLower().Contains(tradeName.ToLower())).ToListAsync();
            var productsDtos = _mapper.Map<List<ProductDTO>>(products);
            return productsDtos;
        }

        public async Task UpdateDescription(string ean, string? description)
        {
            var product = await GetByEan(ean);
            if (product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
            product.Description = description ?? "";
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductPrice(string ean, decimal newPrice)
        {
            var product = await GetByEan(ean);
            if(product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
            product.Price = newPrice;
            await _dbContext.SaveChangesAsync();
        }
        private async Task<Product?> GetByEan(string ean) 
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Ean.ToLower() == ean.ToLower());
        }
    }
}
