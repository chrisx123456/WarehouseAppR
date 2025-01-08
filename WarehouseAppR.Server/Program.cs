using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WarehouseAppR.Server;
using WarehouseAppR.Server.Middleware;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Services;
using WarehouseAppR.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var authenticationSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = "Bearer";
    opt.DefaultScheme = "Bearer";
    opt.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PublicApiPolicy",
        policy =>
        {
            policy.AllowAnyOrigin() // Nadal używamy AllowAnyOrigin, ale z ograniczeniami poniżej
                .WithMethods("GET", "POST", "PUT", "DELETE", "PATCH") // Dozwolone metody
                .WithHeaders("Content-Type", "Authorization", "Accept"); // Dozwolone nagłówki
        });
});



builder.Services.AddSwaggerGen(options =>
{
    // Define the BearerAuth scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Reference the BearerAuth scheme in the operation's security requirements
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
});

builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddDbContext<WarehouseDbContext>(); //DI dla DB
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IManufacturersService, ManufacturerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockDeliveryService, StockDeliveryService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockAndStockDeliveryService, StockAndStockDeliveryService>();
builder.Services.AddScoped<ISellingProductsService, SellingProductsService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();

#if DEBUG
builder.Services.AddScoped<DataSeeder>();
#endif

var app = builder.Build();
app.UseCors("PublicApiPolicy");
app.UseDefaultFiles();
app.UseStaticFiles();

#if DEBUG
using(var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    await services.GetRequiredService<DataSeeder>().Seed();
}
#endif

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
