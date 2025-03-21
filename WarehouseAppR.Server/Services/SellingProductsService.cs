﻿using AutoMapper;
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
        private readonly IProductService _productService;

        public SellingProductsService(WarehouseDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IProductService productService) 
        { 
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
        }
        public async Task ConfirmSale(Guid pendingSaleId)
        {
            var pendingSale = await _dbContext.PendingSales.SingleOrDefaultAsync(ps => ps.PendingSaleId.Equals(pendingSaleId));
            if (pendingSale == null)
                throw new NotFoundException("Pending sale with with such id not found");
            var pendingSaleProducts = pendingSale.PendingSaleProducts.ToList();
            if (pendingSaleProducts == null || !pendingSaleProducts.Any())
                throw new NotFoundException("No items to sale in pending sale");
            
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new LoginException("You need to log in again");

            string? idString = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (idString == null)
                throw new LoginException("you need to log in again");

            List<Sale> sales = new List<Sale>();
            foreach(var psp in pendingSaleProducts)
            {
                await _dbContext.Sales.AddAsync(_mapper.Map<Sale>(psp, opt => opt.Items["UserId"] = Guid.Parse(idString)));
            }
            await _dbContext.SaveChangesAsync();
            foreach (var psp in pendingSaleProducts)
            {

                var baseInStock = psp.Stock;
                if (baseInStock is null) throw new NotFoundException("Internal Error");
                if (baseInStock.Quantity == psp.Quantity)
                {
                    _dbContext.InStock.Remove(baseInStock);
                    //_dbContext.InStock.Where(s => s.StockId ==  baseInStock.StockId).ExecuteDelete();
                }
                else
                {
                    baseInStock.Quantity -= psp.Quantity;
                }
            }
            await _dbContext.Sales.AddRangeAsync(sales);
            await _dbContext.SaveChangesAsync();
            await RejectSale(pendingSaleId);
        }
        public async Task<PendingSalePreviewDTO> GeneratePendingSalePreview(List<NewProductSaleDTO> productsToSale)
        {
            var pendingSale = await GeneratePendingSaleAndPendingSaleProducts(productsToSale);
            var pendingSaleProductsPreviewDtos = _mapper.Map<List<PendingSaleProductPreviewDTO>>(pendingSale.PendingSaleProducts);
            var pendingSalePrieviewDto = new PendingSalePreviewDTO { 
                PendingSaleId = pendingSale.PendingSaleId, 
                ProductPreviews = pendingSaleProductsPreviewDtos 
            };
            return pendingSalePrieviewDto;
        }
        public async Task RejectSale(Guid pendingSaleId)
        {
            var pendingSale = await _dbContext.PendingSales.SingleOrDefaultAsync(ps => ps.PendingSaleId.Equals(pendingSaleId));
            if (pendingSale == null)
                throw new NotFoundException("pendingSale with such id not found");
            var pendingSaleProducts = pendingSale.PendingSaleProducts;
            if (pendingSaleProducts != null && pendingSaleProducts.Any())
                _dbContext.PendingSaleProducts.RemoveRange(pendingSaleProducts);
            _dbContext.PendingSales.Remove(pendingSale);
            await _dbContext.SaveChangesAsync();
        }
        
        private async Task<PendingSale> GeneratePendingSaleAndPendingSaleProducts(List<NewProductSaleDTO> productsToSale)
        {
            List<PendingSaleProduct> pendingSaleProducts = new List<PendingSaleProduct>();
            foreach (NewProductSaleDTO nps in productsToSale)
            {
                if(nps.Count == 0)
                    continue;

                var product = _dbContext.Products
                    .SingleOrDefault(p => p.Ean.Equals(nps.Ean));

                if (product is null)
                    throw new NotFoundException("Product with such ean not found");

                if (product.UnitType == Units.Qt && (int)nps.Count != nps.Count)
                    throw new QuantityTypeAndCountTypeMismatch("Unit qt, count is decimal");

                var productInStock = product.InStock;
                if ((productInStock is null || !productInStock.Any()) || (GetProductCount(productInStock) < nps.Count))
                    throw new NotEnoughInStockException($"Not enough {product.TradeName} in stock / Not in stock at all");

                productInStock = GetSortedList(productInStock);
                pendingSaleProducts.AddRange(GeneratePendingSaleProductList(productInStock, nps.Count));
            }
            if(pendingSaleProducts.Count == 0)
                throw new ForbiddenActionPerformedException("All of the sales had a 0 count");

            var pendingSaleEntry = _dbContext.PendingSales.Add(new PendingSale { DateAdded = DateTime.Now });
            await _dbContext.SaveChangesAsync();
            Guid pendingSaleId = pendingSaleEntry.Entity.PendingSaleId;
            foreach (PendingSaleProduct psp in pendingSaleProducts) psp.PendingSaleId = pendingSaleId;
            await _dbContext.PendingSaleProducts.AddRangeAsync(pendingSaleProducts);
            await _dbContext.SaveChangesAsync();
            return pendingSaleEntry.Entity;
        }
        private List<PendingSaleProduct> GeneratePendingSaleProductList(List<Stock> productInStock, decimal count)
        {
            Guid tempGuid = Guid.NewGuid();
            List<PendingSaleProduct> list = new();
            int i = 0;
            while(count != 0)
            {
                if (productInStock[i].Quantity < count)
                {
                    count -= productInStock[i].Quantity;
                    list.Add(new PendingSaleProduct {
                        PendingSaleId = tempGuid, //Only temporary GUID, just to create an object
                        Quantity = productInStock[i].Quantity, 
                        StockId = productInStock[i].StockId,
                        Stock = productInStock[i]
                        });
                }
                else
                {
                    list.Add(new PendingSaleProduct
                    {
                        PendingSaleId = tempGuid, //Only temporary GUID, just to create an object
                        Quantity = count,
                        StockId = productInStock[i].StockId,
                        Stock = productInStock[i]
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
        private decimal GetProductCount(List<Stock> pList)
        {
            decimal count = 0;
            foreach (Stock s in pList) count += s.Quantity;
            return count;
        }

    }
}
