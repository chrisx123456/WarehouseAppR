using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IAccountService
    {
        public Task AddNewUser(UserDTO user);
        public Task RemoveUser(string email);
        public Task ChangeUserDataAdmin(ChangeUserDataDTO data);
        public Task<string> LoginGetJwt(LoginDTO loginData);
        public Task ChangeEmail(string email, Guid id);
        public Task ChangePassword(PasswordDTO passwordDTO, Guid id);
        public Task ChangeUserPassword(ChangeUserPasswordDTO cup);
        public Task<IEnumerable<ShowUserDTO>> GetAllUsers();
        public Task<ShowUserDTO> GetOwnData(Guid id);
    }
}
