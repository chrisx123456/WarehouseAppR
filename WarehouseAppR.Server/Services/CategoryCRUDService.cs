using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services
{
    public class CategoryCRUDService : ICategoryCRUDService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryCRUDService(WarehouseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void AddNewCategory(CategoryDTO category)
        {
            var item = _dbContext.Categories.FirstOrDefault(c => c.Name == category.Name);
            if (item is not null)
            {
                new ItemAlreadyExistsException("Such category already exists");
            }
            _dbContext.Categories.Add(new Category { Name = category.Name, Vat = category.Vat });
            _dbContext.SaveChanges();
        }

        public void DeleteCategoryByName(string name)
        {
            var toDelete = _dbContext.Categories.SingleOrDefault(c => c.Name == name);
            if (toDelete is null)
            {
                throw new NotFoundException("No category with such name");
            }

            _dbContext.Categories.Remove(toDelete);
            _dbContext.SaveChanges();
        }

        public IEnumerable<CategoryDTO> GetAllCategories()
        {
            var categories = _dbContext.Categories.ToList();
            var categoriesDtos = _mapper.Map<List<CategoryDTO>>(categories);
            return categoriesDtos;
        }

        public IEnumerable<CategoryDTO> GetCategoryByName(string name)
        {
            var categories = _dbContext.Categories.Where(c => c.Name.ToLower().Contains(name.ToLower())).ToList();
            if (!categories.Any()) throw new NotFoundException("No category with such name");
            var categoriesDtos = _mapper.Map<List<CategoryDTO>>(categories);
            return categoriesDtos;
        }

        public void UpdateCategoryVat(string name, int newVat)
        {
            var category = _dbContext.Categories.SingleOrDefault(c => c.Name == name);
            if (category is null)
            {
                throw new NotFoundException("No category with such name");
            }
            category.Vat = newVat;
            _dbContext.SaveChanges();
            
        }
    }
}
