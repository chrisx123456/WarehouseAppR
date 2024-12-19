using Microsoft.AspNetCore.Connections;
using WarehouseAppR.Server.Models.Database;

namespace WarehouseAppR.Server
{
    public class DataSeeder
    {
        private readonly WarehouseDbContext _dbContext;
        public DataSeeder(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (!_dbContext.Database.CanConnect()) throw new Exception("Db can't connect");
            if (!_dbContext.Categories.Any())
            {
                _dbContext.Categories.AddRange(GetCategories());
            }
            if (!_dbContext.Manufacturers.Any())
            {
                _dbContext.Manufacturers.AddRange(GetManufacturers());
            }
            _dbContext.SaveChanges();
            if (!_dbContext.Products.Any())
            {
                _dbContext.Products.AddRange(GetProducts());
            }
            _dbContext.SaveChanges();
        }

        private IEnumerable<Category> GetCategories()
        {
            return new List<Category>{
                new Category { Name="Food", Vat=8},
                new Category { Name="Stationery", Vat=23}
            };
        }
        private IEnumerable<Manufacturer> GetManufacturers() {
            return new List<Manufacturer>
            {
                new Manufacturer{Name="CompanyA"},
                new Manufacturer{Name="CompanyB"}
            };
        }
        private IEnumerable<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    ManufacturerId = (Guid)(_dbContext.Manufacturers.FirstOrDefault(m => m.Name == "CompanyA")?.ManufacturerId),
                    CategoryId = (Guid)_dbContext.Categories.FirstOrDefault(c => c.Name == "Food")?.CategoryId,
                    Name = "Apple",
                    TradeName = "GoldApple",
                    Price = 1.49M,
                    UnitType = Units.Kg,
                    Ean = "12345678"
                },
                new Product
                {
                    ManufacturerId = (Guid)_dbContext.Manufacturers.FirstOrDefault(m => m.Name == "CompanyB")?.ManufacturerId,
                    CategoryId = (Guid)_dbContext.Categories.FirstOrDefault(c => c.Name == "Stationery")?.CategoryId,
                    Name = "Pencil",
                    TradeName = "SuperPencil",
                    Price = 5.23M,
                    UnitType = Units.Qt,
                    Ean = "123456789012"
                }
            };
        }
    }
}
