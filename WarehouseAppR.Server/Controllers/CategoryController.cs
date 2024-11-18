using Microsoft.AspNetCore.Mvc;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }
        //[HttpGet("getAll")]
        //public ActionResult<IEnumerable<Category>> GetAll()
        //{
        //    //return TESTDATA.categories;
        //}
        //[HttpGet("get")]
        //public ActionResult<IEnumerable<Category>> GetByName([FromQuery]string? name)
        //{
        //    if(string.IsNullOrEmpty(name))
        //    {
        //        return RedirectToAction(nameof(GetAll));
        //    }
        //    HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //    return new List<Category> { new Category { CategoryId = Guid.NewGuid(), Name = name, Vat = 99 } };
        //}
    }
}
