using Microsoft.AspNetCore.Mvc;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly WarehouseDbContext _dbContext;
        public StockController(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }



    }
}
