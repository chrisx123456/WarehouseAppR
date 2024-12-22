using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Services
{
    public class StockAndSaleService : IStockAndSalesService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;
        public StockAndSaleService(WarehouseDbContext dbContext, IMapper mapper) 
        { 
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task ConfirmSale(IEnumerable<SaleDTO> sales)
        {
            if (sales is null || !sales.Any()) throw new Exception("Sale list to confirm is empty");
            foreach (SaleDTO s in sales)
            {
                var instock = _dbContext.InStock.SingleOrDefault(st => st.Series.Equals(s.Series));
                if (instock is null) throw new NotFoundException("Confirm sale not found the product in instock table");
                if(instock.Quantity.Equals(s.Quantity))
                {
                    _dbContext.InStock.Remove(instock);
                    await _dbContext.Sales.AddAsync(_mapper.Map<Sale>(s));
                }
                else
                {
                    instock.Quantity -= s.Quantity;
                    await _dbContext.Sales.AddAsync(_mapper.Map<Sale>(s));
                }
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<SaleDTO>> GenerateSellPreview(string ean, decimal count)
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
            List<SaleDTO> saleList = GenerateSaleList(productInStock, count);

            return saleList;
        }
        private List<SaleDTO> GenerateSaleList(List<Stock> inStock, decimal count)
        {
            List<SaleDTO> list = new List<SaleDTO>();
            int i = 0;
            while(count != 0)
            {
                if (inStock[i].Quantity <= count)
                {
                    count -= inStock[i].Quantity;
                    list.Add(_mapper.Map<SaleDTO>(inStock[i], opt => opt.Items["quantity"] = inStock[i].Quantity));
                }
                else
                {
                    list.Add(_mapper.Map<SaleDTO>(inStock[i], opt => opt.Items["quantity"] = count));
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
