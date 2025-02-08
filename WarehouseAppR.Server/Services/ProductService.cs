using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

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
            if (product.InStock.Count() != 0)
                throw new ForbiddenActionPerformedException("Products is in stock, you can't delete it");
            var sales = await _dbContext.Sales.Where(s => s.ProductId == product.ProductId).ToListAsync();
            if (product.StockDeliveries.Count != 0 || sales.Count != 0)
                throw new ForbiddenActionPerformedException("To delete that product contact admin");
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
        // bd -> server -> klient(strona)
        public async Task<IEnumerable<ProductDTO>> GetProductsByName(string name)
        {
            var products = await _dbContext.Products.Where(p => p.Name.ToLower() == name.ToLower()).ToListAsync(); //Materialization Lookup: LINQ to Entities
            //this is LINQ to Objects
            var productsDtos = _mapper.Map<List<ProductDTO>>(products);
            return productsDtos;
            //var productsQueries = _dbContext.Products.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            //string debug = productsQueries.ToQueryString();
            //return null;
        }

        public async Task<ProductDTO> GetProductByTradeName(string tradeName)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.TradeName.ToLower() == tradeName.ToLower());
            var productDto = _mapper.Map<ProductDTO>(product);
            return productDto;
        }

        public async Task UpdateProduct(ProductPatchDTO patchData, string ean)
        {
            if(patchData == null) throw new ArgumentNullException();
            if(patchData.Description == null && patchData.Price == null) throw new ArgumentNullException();

            var prod = await _dbContext.Products.SingleOrDefaultAsync(p => p.Ean == ean);
            if (prod is null) throw new NotFoundException($"Product with {ean} not found");

            if (patchData.Price != null)
            {
                if(patchData.Price > 0)
                {
                    prod.Price = (decimal)patchData.Price;
                }
                else throw new ForbiddenActionPerformedException("Price can't less or equal 0");
            }

            if (patchData.Description != null)
            {
                prod.Description = (string)patchData.Description;
            }
            await _dbContext.SaveChangesAsync();
        }

        //public async Task UpdateDescription(string ean, string? description)
        //{
        //    var product = await GetByEan(ean);
        //    if (product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
        //    product.Description = description ?? "";
        //    await _dbContext.SaveChangesAsync();
        //}

        //public async Task UpdateProductPrice(string ean, decimal newPrice)
        //{
        //    var product = await GetByEan(ean);
        //    if(product is null) throw new NotFoundException($"Product with ean {ean} does not exists");
        //    product.Price = newPrice;
        //    await _dbContext.SaveChangesAsync();
        //}
        private async Task<Product?> GetByEan(string ean) 
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Ean.ToLower() == ean.ToLower());
        }
    }
}
