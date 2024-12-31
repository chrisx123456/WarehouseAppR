using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Services
{
    public class SellingProductsService : ISellingProductsService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SellingProductsService(WarehouseDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
        { 
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task ConfirmSale(Guid pendingSaleId)
        {
            var pendingSale = await _dbContext.PendingSales.SingleOrDefaultAsync(ps => ps.PendingSaleId.Equals(pendingSaleId));
            if (pendingSale == null)
                throw new NotFoundException("Pending sale with with such id not found");
            var productsToSale = await _dbContext.SaleLists.Where(sl => sl.ProductSaleId == pendingSale.ProductSaleId).ToListAsync();
            if (productsToSale == null || !productsToSale.Any())
                throw new NotFoundException("No items to sale in pending sale");
            
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new Exception("No httpcontext");

            string? idString = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idString == null)
                throw new Exception("No user id");
            Guid userId = Guid.Parse(idString);

            List<Sale> sales = new List<Sale>();
            foreach (var pts in productsToSale)
            {
                var product = _dbContext.Products.SingleOrDefault(p => p.Ean.Equals(pts.Ean));
                var instock = _dbContext.InStock.SingleOrDefault(i => i.Series.Equals(pts.Series));
                sales.Add(new Sale
                {
                    UserId = userId,
                    ProductId = product.ProductId,
                    Quantity = pts.Quantity,
                    Price = decimal.Round(pts.Quantity * (product.Price * (1.0M + (decimal)(product.Category.Vat / 100.0M))), 2),
                    DateSaled = DateOnly.FromDateTime(DateTime.Now),
                    Series = pts.Series,
                });
                if (instock is null) throw new NotFoundException("Confirm sale not found the product in instock table");
                if (instock.Quantity.Equals(pts.Quantity))
                {
                    _dbContext.InStock.Remove(instock);
                }
                else
                {
                    instock.Quantity -= pts.Quantity;
                }
            }
            await _dbContext.Sales.AddRangeAsync(sales);
            await RejectSale(pendingSaleId);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<PendingSalePreviewDTO> GeneratePendingSalePreview(List<ProductSaleDTO> productsToSale)
        {
            List<SaleListItemPreviewDTO> salesDtos = new List<SaleListItemPreviewDTO>();
            foreach (ProductSaleDTO ps in productsToSale)
            {
                var product = _dbContext.Products
                    .AsNoTracking()  
                    .SingleOrDefault(p => p.Ean.Equals(ps.Ean));

                if (product is null)
                    throw new NotFoundException("Product with such ean not found");

                var productInStock = product.InStock;
                if (productInStock is null || productInStock.Count == 0)
                    throw new NotEnoughInStockException($"Product {product.TradeName} is not in stock");

                if (product.UnitType == Units.Qt && (int)ps.Count != ps.Count)
                    throw new QuantityTypeAndCountTypeMismatch("Unit qt, count is decimal");

                decimal inStockCount = GetProductCount(productInStock);
                if (inStockCount < ps.Count)
                    throw new NotEnoughInStockException($"Not enough {product.TradeName} in stock");

                productInStock = GetSortedList(productInStock);
                salesDtos.AddRange(GenerateSaleList(productInStock, ps.Count));
            }

            var productSaleId = Guid.NewGuid();
            var pendingSale = new PendingSale
            {
                DateAdded = DateOnly.FromDateTime(DateTime.Now),
                ProductSaleId = productSaleId
            };

            var entry = await _dbContext.PendingSales.AddAsync(pendingSale);

            var saleList = _mapper.Map<List<SaleList>>(salesDtos, opt =>
            {
                opt.Items["ProductSaleId"] = productSaleId;
            });

            await _dbContext.SaleLists.AddRangeAsync(saleList);
            _dbContext.SaveChanges();

            return new PendingSalePreviewDTO
            {
                PendingSales = salesDtos,
                PreviewId = entry.Entity.PendingSaleId
            };

        }
        public async Task RejectSale(Guid pendingSaleId)
        {
            var pendingSale = await _dbContext.PendingSales.SingleOrDefaultAsync(ps => ps.PendingSaleId.Equals(pendingSaleId));
            if (pendingSale == null)
                throw new NotFoundException("pendingSale with such id not found");
            var saleLists = await _dbContext.SaleLists.Where(sl => sl.ProductSaleId == pendingSale.ProductSaleId).ToListAsync();
            if (saleLists == null || !saleLists.Any())
                throw new Exception("saleList empty");
            _dbContext.SaleLists.RemoveRange(saleLists);
            _dbContext.PendingSales.Remove(pendingSale);
            await _dbContext.SaveChangesAsync();
        }
        private List<SaleListItemPreviewDTO> GenerateSaleList(List<Stock> inStock, decimal count)
        {
            List<SaleListItemPreviewDTO> list = new List<SaleListItemPreviewDTO>();
            int i = 0;
            while(count != 0)
            {
                if (inStock[i].Quantity <= count)
                {
                    count -= inStock[i].Quantity;
                    list.Add(_mapper.Map<SaleListItemPreviewDTO>(inStock[i], opt => opt.Items["quantity"] = inStock[i].Quantity));
                }
                else
                {
                    list.Add(_mapper.Map<SaleListItemPreviewDTO>(inStock[i], opt => opt.Items["quantity"] = count));
                    count = 0;
                }
                i++;
            }
            return list;
        }
        private List<Stock> GetSortedList(List<Stock> toSort)
        {
            return toSort.Any(x => x.ExpirationDate.HasValue) ? 
              toSort.OrderBy(x => x.ExpirationDate).ThenBy(x => x.Quantity).ToList() :
              toSort.OrderBy(x => x.Quantity).ToList();
        }
        private decimal GetProductCount(List<Stock>? pList)
        {
            decimal count = 0;
            foreach (Stock s in pList) count += s.Quantity;
            return count;
        }

    }
}
