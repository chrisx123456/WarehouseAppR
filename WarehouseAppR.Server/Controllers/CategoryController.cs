using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryCRUDService)
        {
            _logger = logger;
            _categoryService = categoryCRUDService;
        }
        [HttpGet]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            return Ok(await _categoryService.GetAllCategories());
        }
        [HttpGet("{name}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<CategoryDTO>> GetByName([FromRoute] string name)
        {
            return Ok(await _categoryService.GetCategoryByName(name));
        }
        [HttpDelete("{name}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> DeleteByName([FromRoute]string name)
        {
            await _categoryService.DeleteCategoryByName(name);
            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> AddNewCategory([FromBody] CategoryDTO category)
        {
            await _categoryService.AddNewCategory(category);
            return Ok();
        }
        [HttpPatch("{name}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> EditCategory([FromRoute]string name, [FromQuery]int newVat)
        {
            await _categoryService.UpdateCategoryVat(name, newVat);
            return Ok();
        }
    }
}
