using WarehouseAppR.Server;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<WarehouseDbContext>(); //DI dla DB
builder.Services.AddScoped<DataSeeder>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

using(var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    services.GetRequiredService<DataSeeder>().Seed();
    
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
