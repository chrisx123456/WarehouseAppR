using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using WarehouseAppR.Server.DataAnnotations;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPatch("currency/{currency}")]
        public ActionResult ChangeCurrency([FromRoute]string currency)
        {
            _adminService.SetCurrency(currency);
            return NoContent();
        }
        [HttpGet("currency")]
        [AllowAnonymous]
        public ActionResult<CurrencyDTO> GetCurrency()
        {
            return Ok(_adminService.GetCurrency());
        }
        [HttpDelete("deleteBySeries/{series}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteStockAsync([FromRoute]string series, [FromQuery]bool stockDelivery, [FromQuery]bool sales)
        {
            await _adminService.DeleteBySeries(series, stockDelivery, sales);
            return NoContent();
        }
        [HttpDelete("deleteProduct/{ean}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProductByEan([FromRoute][Ean]string ean)
        {
            await _adminService.DeleteByEan(ean);
            return NoContent();
        }

    }
}
