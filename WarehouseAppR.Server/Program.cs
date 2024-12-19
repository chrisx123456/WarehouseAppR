using WarehouseAppR.Server;
using WarehouseAppR.Server.Middleware;
using WarehouseAppR.Server.Services;
using WarehouseAppR.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WarehouseDbContext>(); //DI dla DB
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IManufacturersService, ManufacturerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockDeliveryService, StockDeliveryService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockAndStockDeliveryService, StockAndStockDeliveryService>();
builder.Services.AddScoped<IStockAndSalesService, StockAndSaleService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));


#if DEBUG
builder.Services.AddScoped<DataSeeder>();
#endif

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

#if DEBUG
using(var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    services.GetRequiredService<DataSeeder>().Seed();
}
#endif

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
