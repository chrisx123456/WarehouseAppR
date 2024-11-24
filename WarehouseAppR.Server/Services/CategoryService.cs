using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly WarehouseDbContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryService(WarehouseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public void AddNewCategory(CategoryDTO category)
        {
            var item = _dbContext.Categories.FirstOrDefault(c => c.Name.ToLower() == category.Name.ToLower());
            if (item is not null)
            {
                throw new ItemAlreadyExistsException("Such category already exists");
            }
            //_dbContext.Categories.Add(new Category { Name = category.Name, Vat = category.Vat });
            _dbContext.Categories.Add(_mapper.Map<Category>(category));
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
