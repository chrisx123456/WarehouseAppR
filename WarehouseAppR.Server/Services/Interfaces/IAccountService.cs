using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IAccountService
    {
        public Task AddNewUser(UserDTO user);
        public Task RemoveUser(string email);
        public Task ChangeUserRole(string email);
        public Task<string> LoginGetJwt(LoginDTO loginData);

    }
}
