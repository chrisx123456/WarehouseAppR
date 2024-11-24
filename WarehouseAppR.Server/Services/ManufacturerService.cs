using AutoMapper;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services
{
    public class ManufacturerService : IManufacturersService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;
        public ManufacturerService(WarehouseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void AddNewManufacturer(ManufacturerDTO manufacturer)
        {
            var item = _dbContext.Manufacturers.FirstOrDefault(m => m.Name.ToLower() == manufacturer.Name.ToLower());
            if (item is not null)
            {
                throw new ItemAlreadyExistsException("Such manufacturer already exists");
            }
            _dbContext.Manufacturers.Add(_mapper.Map<Manufacturer>(manufacturer));
            _dbContext.SaveChanges();
        }

        public void DeleteManufacturerByName(string name)
        {
            var toDelete = _dbContext.Manufacturers.SingleOrDefault(m => m.Name == name);
            if (toDelete is null)
            {
                throw new NotFoundException("No manufacturer with such name");
            }

            _dbContext.Manufacturers.Remove(toDelete);
            _dbContext.SaveChanges();
        }

        public IEnumerable<ManufacturerDTO> GetAllManufacturers()
        {
            var manufacturers = _dbContext.Manufacturers.ToList();
            var manufacturersDtos = _mapper.Map<List<ManufacturerDTO>>(manufacturers);
            return manufacturersDtos;
        }

        public IEnumerable<ManufacturerDTO> GetManufacturerByName(string name)
        {
            var manufacturers = _dbContext.Manufacturers.Where(m => m.Name.ToLower().Contains(name.ToLower())).ToList();
            if (!manufacturers.Any()) throw new NotFoundException("No manufacturer with such name");
            var manufacturersDtos = _mapper.Map<List<ManufacturerDTO>>(manufacturers);
            return manufacturersDtos;
        }
    }
}
