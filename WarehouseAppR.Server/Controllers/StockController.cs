using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.DataAnnotations;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        public StockController(WarehouseDbContext dbContext, IStockService stockService)
        {
            _stockService = stockService;
        }
        [HttpGet("{ean}")]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetAllInStock([FromRoute][Ean] string ean)
        {
            await _stockService.GetInStockByEan(ean);
            return null;
        }

    }
}
