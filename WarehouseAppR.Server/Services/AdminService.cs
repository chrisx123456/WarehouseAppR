using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Services
{
    public class AdminService : IAdminService
    {
        private readonly WarehouseDbContext _dbContext;
        public AdminService(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteByEan(string ean)
        {
            var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Ean == ean);
            if (product == null)
                throw new NotFoundException("Product with such ean not found");
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBySeries(string series, bool stockDelivery, bool sales)
        {
            string warning = string.Empty;
            var stock = await _dbContext.InStock.SingleOrDefaultAsync(s => s.Series.ToLower() == series.ToLower());
            if (stock == null)
                throw new NotFoundException("Stock with such series not found");
            _dbContext.InStock.Remove(stock);
            await _dbContext.SaveChangesAsync();
            if (stockDelivery)
            {
                var stockDeliveryI = await _dbContext.StockDeliveries.SingleOrDefaultAsync(sd => sd.Series.ToLower() == series.ToLower());
                if (stockDeliveryI != null)
                {
                    _dbContext.StockDeliveries.Remove(stockDeliveryI);
                    await _dbContext.SaveChangesAsync();
                }
                else warning += "Warning: Stock delivery with such series not found\n";

            }
            if (sales)
            {
                var salesI = await _dbContext.Sales.Where(s => s.Series.ToLower() == series.ToLower()).ToListAsync();
                if (salesI != null && salesI.Count != 0)
                {
                    _dbContext.Sales.RemoveRange(salesI);
                    await _dbContext.SaveChangesAsync();
                }
                else warning += "Warning: Sales with such series not found\n";
            }
            if (warning != string.Empty)
                throw new NotFoundException(warning);
        }

        public CurrencyDTO GetCurrency()
        {
            string json = File.ReadAllText("settings.json");
            JsonNode? jn = JsonNode.Parse(json);
            string? currnecy = jn?["CurrencyName"].GetValue<string>();
            if (currnecy == null || !currnecy.Any())
                throw new NotFoundException("Currency not found");
            return new CurrencyDTO { Currency = currnecy };
        }
        public void SetCurrency(string currency)
        {
            string json = File.ReadAllText("settings.json");
            JsonNode? jn = JsonNode.Parse(json);
            jn!["CurrencyName"] = currency;
            File.WriteAllText("settings.json", jn?.ToJsonString());
        }
    }
}
