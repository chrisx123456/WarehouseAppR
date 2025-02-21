using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using WarehouseAppR.Server.Exceptions;
using WarehouseAppR.Server.Models.Database;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

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
        public async Task AddNewCategory(CategoryDTO category)
        {
            var item = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == category.Name.ToLower());
            if (item is not null)
            {
                throw new ItemAlreadyExistsException("Such category already exists");
            }
            //_dbContext.Categories.Add(new Category { Name = category.Name, Vat = category.Vat });
            await _dbContext.Categories.AddAsync(_mapper.Map<Category>(category));
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryByName(string name)
        {
            
            var toDelete = await _dbContext.Categories.SingleOrDefaultAsync(c => c.Name == name);
            if (toDelete is null)
                throw new NotFoundException("No category with such name");
            var isThereProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.CategoryId == toDelete.CategoryId);
            if (isThereProduct is not null)
                throw new ForbiddenActionPerformedException("Some products use this category, you cant delete it");

            _dbContext.Categories.Remove(toDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var categoriesDtos = _mapper.Map<List<CategoryDTO>>(categories);
            return categoriesDtos;
        }

        public async Task<CategoryDTO> GetCategoryByName(string name)
        {
            var category = await _dbContext.Categories.SingleOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
            if (category == null) throw new NotFoundException("No category with such name");
            var categoryDto = _mapper.Map<CategoryDTO>(category);

            //var categories = await _dbContext.Categories
            //    .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
            //    .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
            //    .ToListAsync();
            return categoryDto;
        }

        public async Task UpdateCategoryVat(string name, int newVat)
        {
            var category = await _dbContext.Categories.SingleOrDefaultAsync(c => c.Name == name);
            if (category is null)
            {
                throw new NotFoundException("No category with such name");
            }
            category.Vat = newVat;
            await _dbContext.SaveChangesAsync();
        }
    }
}
