﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models;
using WarehouseAppR.Server.Services.Interfaces;

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
        public async Task AddNewManufacturer(ManufacturerDTO manufacturer)
        {
            var item = await _dbContext.Manufacturers.FirstOrDefaultAsync(m => m.Name.ToLower() == manufacturer.Name.ToLower());
            if (item is not null)
            {
                throw new ItemAlreadyExistsException("Such manufacturer already exists");
            }
            await _dbContext.Manufacturers.AddAsync(_mapper.Map<Manufacturer>(manufacturer));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteManufacturerByName(string name)
        {
            var toDelete = await _dbContext.Manufacturers.SingleOrDefaultAsync(m => m.Name == name);
            if (toDelete is null)
            {
                throw new NotFoundException("No manufacturer with such name");
            }

            _dbContext.Manufacturers.Remove(toDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ManufacturerDTO>> GetAllManufacturers()
        {
            var manufacturers = _dbContext.Manufacturers.ToListAsync();
            var manufacturersDtos = _mapper.Map<List<ManufacturerDTO>>(manufacturers);
            return manufacturersDtos;
        }

        public async Task<IEnumerable<ManufacturerDTO>> GetManufacturerByName(string name)
        {
            var manufacturers = await _dbContext.Manufacturers.Where(m => m.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            if (!manufacturers.Any()) throw new NotFoundException("No manufacturer with such name");
            var manufacturersDtos = _mapper.Map<List<ManufacturerDTO>>(manufacturers);
            return manufacturersDtos;
        }
    }
}
