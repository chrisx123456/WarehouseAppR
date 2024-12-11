using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.DataAnnotations;
using WarehouseAppR.Server.Models;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IStockDeliveryService _stockDeliveryService;
        public DeliveryController(IStockDeliveryService stockDeliveryService)
        {
            _stockDeliveryService = stockDeliveryService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddNewStockDeliveryDTO>>> GetAllDeliveries()
        {
            var deliveries = await _stockDeliveryService.GetAllDeliveries();
            return Ok(deliveries);
        }
        [HttpGet("ean/{ean}")]
        public async Task<ActionResult<IEnumerable<StockDeliveryDTO>>> GetDeliveriesByEan([FromRoute][Ean] string ean)
        {
            var deliveries = await _stockDeliveryService.GetDeliveriesByEan(ean);
            return Ok(deliveries);
        }
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<StockDeliveryDTO>>> GetDeliveriesByDate([FromRoute]DateOnly date)
        {
            var deliveries = await _stockDeliveryService.GetDeliveriesByDate(date);
            return Ok(deliveries);
        }
        [HttpPost]
        public async Task<ActionResult> AddDelivery([FromBody] AddNewStockDeliveryDTO newStock)
        {
            await _stockDeliveryService.AddDelivery(newStock);
            return NoContent();
        }

    }
}
