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
            var userdb = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == user.Email.ToLower());
            if (userdb is not null) throw new LoginException("User with such email exists");
            User newUser;
            try
            {
                newUser = new User()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleId = _dbContext.Roles.SingleOrDefaultAsync(u => u.RoleName.ToLower() == user.RoleName.ToLower()).Result.RoleId,
                };
            }
            catch
            {
                throw new NotFoundException("Role with such name not found");
            }
            var passHash = _passwordHasher.HashPassword(newUser, user.Password);
            newUser.PasswordHash = passHash;
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeUserRole(string email, string role)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user is  null) throw new NotFoundException("User with such email not found");
            Guid roleId;
            try
            {
                roleId = _dbContext.Roles.SingleOrDefaultAsync(u => u.RoleName.ToLower() == role.ToLower()).Result.RoleId;
            }
            catch
            {
                throw new NotFoundException("Role with such name not found");
            }
            user.RoleId = roleId;
            await _dbContext.SaveChangesAsync();
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

        public async Task RemoveUser(string email)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user is null) throw new NotFoundException("User with such email not found");
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeEmail(string email, Guid id)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserId == id);
            if(user.Email.ToLower() == email.ToLower()) throw new ItemAlreadyExistsException("This email is already taken");
            if (user is null) throw new Exception("Jwt token user claim id not found in db");
            user.Email = email;
            await _dbContext.SaveChangesAsync();
        }
    }
}
