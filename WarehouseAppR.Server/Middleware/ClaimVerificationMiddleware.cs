
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Middleware
{
    public class ClaimVerificationMiddleware : IMiddleware
    {
        private readonly WarehouseDbContext _dbContext;
        public ClaimVerificationMiddleware(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if(await Verify(context))
            {
                await next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDTO { Message = "You need to log in again" }));
            }
        }
        private async Task<bool> Verify(HttpContext context)
        {
            string? id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(id == null) return true;
            var claims = context.User.Claims;
            if (claims == null || !claims.Any()) return false;
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserId == new Guid(id));
            if (user == null) return false;
            if($"{user.FirstName} {user.LastName}" != context.User.FindFirst(ClaimTypes.Name)?.Value)
                return false;
            if(user.Role.RoleName != context.User.FindFirst(ClaimTypes.Role)?.Value)
                return false;
            return true;
        }
    }
}
