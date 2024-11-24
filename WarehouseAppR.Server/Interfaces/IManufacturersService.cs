using WarehouseAppR.Server.DTOs;

namespace WarehouseAppR.Server.Interfaces
{
    public interface IManufacturersService
    {
        public IEnumerable<ManufacturerDTO> GetAllManufacturers();
        public IEnumerable<ManufacturerDTO> GetManufacturerByName(string name);
        public void DeleteManufacturerByName(string name);
        public void AddNewManufacturer(ManufacturerDTO category);
    }
}
