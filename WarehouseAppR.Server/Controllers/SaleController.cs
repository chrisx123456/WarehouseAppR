using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;
using WarehouseAppR.Server.DataAnnotations;
using WarehouseAppR.Server.Exceptions;
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
        private readonly ISellingProductsService _sellingProductsService;
        private readonly ISaleService _saleService;
        public SaleController(ISellingProductsService stockAndSalesService, ISaleService saleService)
        {
            _sellingProductsService = stockAndSalesService;
            _saleService = saleService;
        }
        [HttpGet]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult<IEnumerable<SaleDTO>>> GetAllSales()
        {
            var saleDtos = await _saleService.GetAllSales();
            return Ok(saleDtos);
        }
        [HttpPost("GeneratePendingSales")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<PendingSalePreviewDTO>> GenerateSellPreview([FromBody]IEnumerable<ProductSaleDTO> productsToSale)
        {
            var preview = await _sellingProductsService.GeneratePendingSalePreview(productsToSale.ToList());
            return Ok(preview);
        }
        [HttpPost("confirm/{previewId}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult> ConfirmSale([FromRoute]Guid previewId)
        {
            await _sellingProductsService.ConfirmSale(previewId);
            return NoContent();
        }
        [HttpPost("reject/{previewId}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult> RejectSale([FromRoute] Guid previewId)
        {
            await _sellingProductsService.RejectSale(previewId);
            return NoContent();
        }
        //[HttpGet("/user")]
        //[Authorize(Roles = "User,Manager,Admin")]
        //public Task<ActionResult<IEnumerable<>>> GetUserSales()
        //{
        //    string? id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (id == null) throw new LoginException("You're not logged in");
        //    return Ok();
        //}

    }
}
