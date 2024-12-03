using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryDTO>> GetAllCategories();
        public Task<IEnumerable<CategoryDTO>> GetCategoryByName(string name);
        public Task DeleteCategoryByName(string name);
        public Task AddNewCategory(CategoryDTO category);
        public Task UpdateCategoryVat(string name, int newVat);
    }
}
