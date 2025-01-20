using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WarehouseAppR.Server.Exceptions;
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
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenDTO>> LoginUser([FromBody] LoginDTO user)
        {
            string token = await _accountService.LoginGetJwt(user);
            return Ok(new TokenDTO { Token = token});
        }
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> RegisterUser([FromBody] UserDTO user)
        {
            await _accountService.AddNewUser(user);
            return Ok("Tet");
        }
        [HttpDelete("delete/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser([FromRoute] string email)
        {
            await _accountService.RemoveUser(email);
            return Ok();
        }
        [HttpPatch("newemail/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult> ChangeEmail([FromRoute] string email)
        {
            string? id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null) throw new LoginException("You're not logged in");
            await _accountService.ChangeEmail(email, new Guid(id));
            return Ok();
        }
        [HttpPatch("newpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ChangePassword([FromBody] PasswordDTO passwordDTO)
        {
            string? id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null) throw new LoginException("You're not logged in");
            await _accountService.ChangePassword(passwordDTO, new Guid(id));
            return Ok();
        }
        [HttpGet("getrole")]
        public ActionResult<RoleDTO> GetRole()
        {
            string? role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == null) throw new LoginException("You're not logged in");
            return Ok(new RoleDTO { Role = role});
        }
        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserData([FromBody] ChangeUserDataDTO cud)
        {
            await _accountService.ChangeUserDataAdmin(cud);
            return Ok();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ShowUserDTO>>> GetAllUsers()
        {
            return Ok(await _accountService.GetAllUsers());
        }

    }
}
