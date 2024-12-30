using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.DataAnnotations;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IStockDeliveryService _stockDeliveryService;
        private readonly IStockAndStockDeliveryService _stockAndDeliveryService;
        public DeliveryController(IStockDeliveryService stockDeliveryService, IStockAndStockDeliveryService stockAndStockDeliveryService)
        {
            _stockAndDeliveryService = stockAndStockDeliveryService;
            _stockDeliveryService = stockDeliveryService;
        }
        [HttpGet]
        [Authorize(Roles = "User,Manager,Admin")]

        public async Task<ActionResult<IEnumerable<AddNewStockDeliveryDTO>>> GetAllDeliveries()
        {
            var deliveries = await _stockDeliveryService.GetAllDeliveries();
            return Ok(deliveries);
        }
        [HttpGet("ean/{ean}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<IEnumerable<StockDeliveryDTO>>> GetDeliveriesByEan([FromRoute][Ean] string ean)
        {
            var deliveries = await _stockDeliveryService.GetDeliveriesByEan(ean);
            return Ok(deliveries);
        }
        [HttpGet("date/{date}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<IEnumerable<StockDeliveryDTO>>> GetDeliveriesByDate([FromRoute]DateOnly date)
        {
            var deliveries = await _stockDeliveryService.GetDeliveriesByDate(date);
            return Ok(deliveries);
        }
        [HttpPost]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult> AddDelivery([FromBody] AddNewStockDeliveryDTO newStock)
        {
            await _stockAndDeliveryService.AddDelivery(newStock);
            return NoContent();
        }

    }
}
