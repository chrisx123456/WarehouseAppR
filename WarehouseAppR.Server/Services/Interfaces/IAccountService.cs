using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IAccountService
    {
        public Task AddNewUser(UserDTO user);
        public Task RemoveUser(string email);
        public Task ChangeUserRole(string email, string role);
        public Task<string> LoginGetJwt(LoginDTO loginData);
        public Task ChangeEmail(string email, Guid id);
        public Task ChangePassword(string password, Guid id);

    }
}
