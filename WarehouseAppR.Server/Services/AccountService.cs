using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        public AccountService(WarehouseDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }
        public async Task AddNewUser(UserDTO user)
        {
            var newUser = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleId = Guid.NewGuid()
            };
            var passHash = _passwordHasher.HashPassword(newUser, user.Password);
            newUser.PasswordHash = passHash;
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        public Task ChangeUserRole(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<string> LoginGetJwt(LoginDTO loginData)
        {
            var user = await _dbContext.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Email.Equals(loginData.Email));
            if (user is null) throw new LoginException("Invalid email or password");
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginData.Password);
            if(result == PasswordVerificationResult.Failed) throw new LoginException("Invalid email or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.RoleName}")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var daysValid = DateTime.Now.AddDays(_authenticationSettings.JwtValidDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, null, daysValid, creds);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public Task RemoveUser(string email)
        {
            throw new NotImplementedException();
        }
    }
}
