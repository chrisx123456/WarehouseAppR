using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.DataAnnotations;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly IStockAndSalesService _stockAndSalesService;
        private readonly ISaleService _saleService;
        private static Dictionary<Guid, IEnumerable<SaleDTO>> _pendingSales = new();
        public SaleController(IStockAndSalesService stockAndSalesService, ISaleService saleService)
        {
            _stockAndSalesService = stockAndSalesService;
            _saleService = saleService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDTO>>> GetAllSales()
        {
            var saleDtos = await _saleService.GetAllSales();
            return Ok(saleDtos);
        }
        [HttpGet("{ean}")]
        public async Task<ActionResult<PendingSaleDTO>> GenerateSellPreview([FromRoute][Ean] string ean, [FromQuery]decimal count)
        {
            var sellList = await _stockAndSalesService.GenerateSellPreview(ean, count);
            var previewId = Guid.NewGuid();
            _pendingSales.Add(previewId, sellList);
            var pendingSales = new PendingSaleDTO { PreviewId = previewId , PendingSales = sellList};
            return Ok(pendingSales);
        }
        [HttpPost("confirm/{previewId}")]
        public async Task<ActionResult> ConfirmSale([FromRoute]Guid previewId)
        {
            await _stockAndSalesService.ConfirmSale(_pendingSales[previewId]);
            _pendingSales.Remove(previewId);
            return NoContent();
        }
        [HttpPost("reject/{previewId}")]
        public async Task<ActionResult> RejectSale([FromRoute] Guid previewId)
        {
            _pendingSales.Remove(previewId);
            return NoContent();
        }

    }
}
