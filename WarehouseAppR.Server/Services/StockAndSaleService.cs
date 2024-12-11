using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Services
{
    public class StockAndSaleService : IStockAndSalesService
    {
        public readonly WarehouseDbContext _dbContext;
        public StockAndSaleService(WarehouseDbContext dbContext) 
        { 
            _dbContext = dbContext;
        
        }
        public async Task<IEnumerable<Sale>> Sell(string ean, decimal count)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Ean.Equals(ean));
            if (product is null) throw new NotFoundException("Product with such ean not found");
            var productInStock = product.InStock;
            if (productInStock is null || productInStock.Count == 0) throw new NotEnoughInStockException($"Product {product.TradeName} is not in stock");
            if (product.UnitType == Units.Qt)
                if ((int)count != count) throw new QuantityTypeAndCountTypeMismatch("Unit qt, count is decimal");

            decimal inStockCount = GetProductCount(productInStock);
            if (inStockCount < count) throw new NotEnoughInStockException($"Not enough {product.TradeName} in stock");
            productInStock = GetSortedList(productInStock);
            List<Sale> saleList = GenerateSaleList(productInStock, count);
            return saleList;
        }
        private List<Sale> GenerateSaleList(List<Stock> inStock, decimal count)
        {
            List<Sale> list = new List<Sale>();
            int i = 0;
            while(count != 0)
            {
                if (inStock[i].Quantity <= count)
                {
                    count -= inStock[i].Quantity;
                    list.Add(new Sale
                    {
                        UserId = 1,
                        ProductId = inStock[i].ProductId,
                        Quantity = inStock[i].Quantity,
                        DateSaled = DateOnly.FromDateTime(DateTime.Now),
                        Series = inStock[i].Series,
                        Price = inStock[i].Quantity * (inStock[i].Product.Price * (1.0M + inStock[i].Product.Category.Vat / 100.0M)),
                    });
                }
                else
                {
                    list.Add(new Sale
                    {
                        UserId = 1,
                        ProductId = inStock[i].ProductId,
                        Quantity = count,
                        DateSaled = DateOnly.FromDateTime(DateTime.Now),
                        Series = inStock[i].Series,
                        Price = count * (inStock[i].Product.Price * (1.0M + inStock[i].Product.Category.Vat / 100.0M)),
                    });
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
