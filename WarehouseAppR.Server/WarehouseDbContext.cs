using Microsoft.EntityFrameworkCore;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server
{
    public class WarehouseDbContext : DbContext 
    {
        
        private string _connectionString = "workstation id=uniwarehousedb.mssql.somee.com;packet size=4096;user id=maciek;pwd=maciek123;data source=uniwarehousedb.mssql.somee.com;persist security info=False;initial catalog=uniwarehousedb;TrustServerCertificate=True";
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Stock> InStock { get; set; }
        public DbSet<StockDelivery> StockDeliveries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
