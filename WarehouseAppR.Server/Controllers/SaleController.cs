using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.DataAnnotations;
using WarehouseAppR.Server.Models;
using WarehouseAppR.Server.Services;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly IStockAndSalesService _stockAndSalesService;
        public SaleController(IStockAndSalesService stockAndSalesService)
        {
            _stockAndSalesService = stockAndSalesService;
        }
        [HttpGet("{ean}")]
        public async Task<ActionResult<IEnumerable<Sale>>> SellTest([FromRoute][Ean] string ean, [FromQuery]decimal count)
        {
            var sellList = await _stockAndSalesService.Sell(ean, count);
            return Ok(sellList);
        }
    }
}
