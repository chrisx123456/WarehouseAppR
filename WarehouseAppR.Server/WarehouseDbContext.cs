using Microsoft.EntityFrameworkCore;
using WarehouseAppR.Server.Models.Database;

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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PendingSale> PendingSales { get; set; }
        public DbSet<SaleList> SaleLists { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>()
                .HasIndex(s => s.Series)
                .IsUnique();

            modelBuilder.Entity<StockDelivery>()
                .HasIndex(sd => sd.Series)
                .IsUnique();

            // Konfiguracja relacji 1:1 opcjonalnej po stronie Stock
            modelBuilder.Entity<StockDelivery>()
                .HasOne(sd => sd.Stock)
                .WithOne(s => s.StockDelivery)
                .HasForeignKey<StockDelivery>(sd => sd.Series)
                .HasPrincipalKey<Stock>(s => s.Series)
                .IsRequired(false); // Relacja opcjonalna po stronie Stock
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
