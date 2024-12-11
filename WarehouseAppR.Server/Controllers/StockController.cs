using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.DataAnnotations;
using WarehouseAppR.Server.Models;
using WarehouseAppR.Server.Services.Interfaces;

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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetAllInStock()
        {
            var inStock = await _stockService.GetAllInStock();
            return Ok(inStock);
        }
        [HttpGet("ean/{ean}")]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetInStockByEan([FromRoute][Ean] string ean)
        {
            var inStock = await _stockService.GetInStockByEan(ean);
            return Ok(inStock);
        }
        [HttpGet("series/{series}")]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetInStockBySeries([FromRoute] string series)
        {
            var inStock = await _stockService.GetInStockBySeries(series);
            return Ok(inStock);
        }
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<StockDTO>>> GetInStockFromDate([FromRoute] DateOnly date)
        {
            var inStock = _stockService.GetInStockFromDate(date);
            return Ok(inStock);
        }


    }
}
