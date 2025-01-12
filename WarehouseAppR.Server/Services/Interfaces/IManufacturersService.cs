using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface IManufacturersService
    {
        public Task<IEnumerable<ManufacturerDTO>> GetAllManufacturers();
        public Task<IEnumerable<ManufacturerDTO>> GetManufacturerByName(string name);
        public Task DeleteManufacturerByName(string name);
        public Task AddNewManufacturer(ManufacturerDTO category);
        public Task UpdateManufacturerName(string name, string newName);
    }
}
