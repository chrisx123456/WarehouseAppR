using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using WarehouseAppR.Server.Models;
using WarehouseAppR.Server.Models.DTO;

namespace WarehouseAppR.Server.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<IEnumerable<CategoryDTO>> GetAllCategories();
        public Task<CategoryDTO> GetCategoryByName(string name);
        public Task DeleteCategoryByName(string name);
        public Task AddNewCategory(CategoryDTO category);
        public Task UpdateCategoryVat(string name, int newVat);
    }
}
