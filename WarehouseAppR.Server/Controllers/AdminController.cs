using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
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

    }
}
