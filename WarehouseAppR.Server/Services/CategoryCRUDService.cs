using System.Collections;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Services
{
    public class CategoryCRUDService : ICategoryCRUDService
    {
        private readonly WarehouseDbContext _dbContext;
        public CategoryCRUDService(WarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void AddNewCategory(CategoryDTO category)
        {
            _dbContext.Categories.Add(new Category { Name = category.Name, Vat = category.Vat });
            _dbContext.SaveChanges();
        }

        public void DeleteCategoryByName(string name)
        {
            Category toDelete = _dbContext.Categories.Single(c => c.Name == name);
            _dbContext.Categories.Remove(toDelete);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
        }

        public IEnumerable<Category> GetCategoryByName(string name)
        {
            return _dbContext.Categories.Where(c => c.Name.ToLower().Contains(name.ToLower())).ToList();
        }

        public void UpdateCategoryVat(string name, int newVat)
        {
          _dbContext.Categories.Single(c => c.Name == name).Vat = newVat;
          _dbContext.SaveChanges();
            
        }
    }
}
