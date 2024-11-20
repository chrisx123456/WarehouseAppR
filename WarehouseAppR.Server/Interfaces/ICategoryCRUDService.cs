using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Interfaces
{
    public interface ICategoryCRUDService
    {
        public IEnumerable<CategoryDTO> GetAllCategories();
        public IEnumerable<CategoryDTO> GetCategoryByName(string name);
        public void DeleteCategoryByName(string name);
        public void AddNewCategory(CategoryDTO category);
        public void UpdateCategoryVat(string name, int newVat);
    }
}
