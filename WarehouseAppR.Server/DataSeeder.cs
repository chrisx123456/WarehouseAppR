using Microsoft.AspNetCore.Connections;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server
{
    public class DataSeeder
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IAccountService _accountService;
        public DataSeeder(WarehouseDbContext dbContext, IAccountService accountService)
        {
            _dbContext = dbContext;
            _accountService = accountService;
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
            if (!_dbContext.Roles.Any())
            {
                _dbContext.Roles.AddRange(GetRoles());
            }
            _dbContext.SaveChanges();
            if (!_dbContext.Users.Any())
            {
                AddAdminUser();
            }
        }
        private void AddAdminUser()
        {
            _accountService.AddNewUser(new Models.DTO.UserDTO
            {
                Email = "t@t.pl",
                FirstName = "t",
                LastName = "t",
                Password = "t",
                RoleName = "Admin",
            });
        }
        private IEnumerable<Role> GetRoles()
        {
            return new List<Role>()
            {
                new Role { RoleName = "User" },
                new Role { RoleName = "Manager" },
                new Role { RoleName = "Admin" }
            };
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
