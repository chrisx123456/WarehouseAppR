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
            TokenDTO token = await _accountService.LoginGetJwt(user);
            return token;
        }
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RegisterUser([FromBody] UserDTO user)
        {
            await _accountService.AddNewUser(user);
            return Ok();
        }
        [HttpDelete("delete/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUserByEmail([FromRoute] string email)
        {
            await _accountService.DeleteUserByEmail(email);
            return Ok();
        }
        [HttpPatch("newemail/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult> ChangeOwnEmail([FromRoute] string email)
        {
            string? id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null) throw new LoginException("You're not logged in");
            await _accountService.ChangeEmail(email, new Guid(id));
            return Ok();
        }
        [HttpPatch("newpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ChangeOwnPassword([FromBody] PasswordDTO password)
        {
            string? id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null) throw new LoginException("You're not logged in");
            await _accountService.ChangePassword(password, new Guid(id));
            return Ok();
        }
        [HttpGet("getrole")]
        [Authorize(Roles = "Admin,Manager,User")]
        public ActionResult<RoleDTO> GetOwnRole()
        {
            string? role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == null) throw new LoginException("You're not logged in");
            return Ok(new RoleDTO { Role = role});
        }
        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserData([FromBody] ChangeUserDataDTO data)
        {
            await _accountService.ChangeUserDataAdmin(data);
            return Ok();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ShowUserDTO>>> GetAllUsers()
        {
            return Ok(await _accountService.GetAllUsers());
        }
        [HttpGet("owndata")]
        [AllowAnonymous]
        public async Task<ActionResult<ShowUserDTO>> GetOwnData()
        {
            string? id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null) throw new LoginException("You're not logged in");
            return Ok(await _accountService.GetOwnData(new Guid(id)));
        }

    }
}
