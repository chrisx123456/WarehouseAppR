using WarehouseAppR.Server.DTOs;

namespace WarehouseAppR.Server.Interfaces
{
    public interface IManufacturersService
    {
        public Task<IEnumerable<ManufacturerDTO>> GetAllManufacturers();
        public Task<IEnumerable<ManufacturerDTO>> GetManufacturerByName(string name);
        public Task DeleteManufacturerByName(string name);
        public Task AddNewManufacturer(ManufacturerDTO category);
    }
}
