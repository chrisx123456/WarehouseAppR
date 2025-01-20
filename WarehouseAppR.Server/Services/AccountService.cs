using AutoMapper;
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
        private readonly IMapper _mapper;
        public AccountService(WarehouseDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _mapper = mapper;
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

        //public async Task ChangeUserRole(string email, string role)
        //{
        //    var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        //    if (user is  null) throw new NotFoundException("User with such email not found");
        //    Guid roleId;
        //    try
        //    {
        //        roleId = _dbContext.Roles.SingleOrDefaultAsync(u => u.RoleName.ToLower() == role.ToLower()).Result.RoleId;
        //    }
        //    catch
        //    {
        //        throw new NotFoundException("Role with such name not found");
        //    }
        //    user.RoleId = roleId;
        //    await _dbContext.SaveChangesAsync();
        //}
        public async Task ChangeUserDataAdmin(ChangeUserDataDTO data)
        {
            if (string.IsNullOrEmpty(data.OldEmail) || (data.Password == null && data.RoleName == null && data.Email == null && data.FirstName == null && data.LastName == null))
                throw new ForbiddenActionPerformedException("No data to change");

            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == data.OldEmail.ToLower());
            if (user == null) throw new NotFoundException("User with such email not found");
            if(data.Email != null)
                user.Email = data.Email;
            if (data.RoleName != null)
                user.Role = await _dbContext.Roles.SingleOrDefaultAsync(r => r.RoleName.ToLower() == data.RoleName.ToLower());
            if(data.FirstName != null)
                user.FirstName = data.FirstName;
            if(data.LastName != null)
                user.LastName = data.LastName;
            await _dbContext.SaveChangesAsync();
            if (data.Password != null)
                user.PasswordHash = _passwordHasher.HashPassword(user, data.Password);
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
            if (user is null) throw new LoginException("You're not logged in");
            if (user.Email.ToLower() == email.ToLower()) throw new ItemAlreadyExistsException("This email is already taken");
            if (user is null) throw new Exception("Jwt token user claim id not found in db");
            user.Email = email;
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangePassword(PasswordDTO passwordDTO, Guid id)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserId == id);
            if (user is null) throw new LoginException("You're not logged in");
            var passHash = _passwordHasher.HashPassword(user, passwordDTO.Password);
            user.PasswordHash = passHash;
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangeUserPassword(ChangeUserPasswordDTO cup)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == cup.Email);
            if (user == null) throw new NotFoundException("User with such email not found");
            user.PasswordHash = _passwordHasher.HashPassword(user, cup.Password);
        }

        public async Task<IEnumerable<ShowUserDTO>> GetAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            if (!users.Any()) throw new NotFoundException("No users found");
            var usersDtos = _mapper.Map<List<ShowUserDTO>>(users);
            return usersDtos;
        }
         

    }
}
