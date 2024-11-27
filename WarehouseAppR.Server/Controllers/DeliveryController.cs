using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

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
        public ActionResult<IEnumerable<StockDeliveryDTO>> GetAllDeliveryies()
        {

            return Ok();
        }
    }
}
