using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryCRUDService _categoryCRUDService;
        public CategoryController(ILogger<CategoryController> logger, ICategoryCRUDService categoryCRUDService)
        {
            _logger = logger;
            _categoryCRUDService = categoryCRUDService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> GetAll()
        {
            return Ok(_categoryCRUDService.GetAllCategories());
        }
        [HttpGet("/{name}")]
        public ActionResult<CategoryDTO> GetByName([FromRoute] string name)
        {
            return Ok(_categoryCRUDService.GetCategoryByName(name));
        }
        [HttpDelete("/{name}")]
        public ActionResult DeleteByName([FromRoute]string name)
        {
            _categoryCRUDService.DeleteCategoryByName(name);
            return NoContent();
        }
        [HttpPost]
        public ActionResult AddNewCategory([FromBody] CategoryDTO category)
        {
            _categoryCRUDService.AddNewCategory(category);
            return Ok();
        }
        [HttpPatch("/{name}")]
        public ActionResult EditCategory([FromRoute]string name, [FromQuery]int newVat)
        {
            _categoryCRUDService.UpdateCategoryVat(name, newVat);
            return Ok();
        }
    }
}
