using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost]
        public async Task<ActionResult<string>> AddNewUserAsync([FromBody] LoginDTO user)
        {
            string token = await _accountService.LoginGetJwt(user);
            return Ok(token);
        }
    }
}
