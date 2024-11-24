using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.DTOs;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Services;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturersService _manufacturerCRUDService;
        public ManufacturerController(IManufacturersService manufacturerCRUDService)
        {
            _manufacturerCRUDService = manufacturerCRUDService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<ManufacturerDTO>> GetAll()
        {
            return Ok(_manufacturerCRUDService.GetAllManufacturers());
        }
        [HttpGet("{name}")]
        public ActionResult<ManufacturerDTO> GetByName([FromRoute] string name)
        {
            return Ok(_manufacturerCRUDService.GetManufacturerByName(name));
        }
        [HttpDelete("{name}")]
        public ActionResult Delete([FromRoute] string name)
        {
            _manufacturerCRUDService.DeleteManufacturerByName(name);
            return NoContent();
        }
        [HttpPost]
        public ActionResult AddNew([FromBody] ManufacturerDTO manufacturer)
        {
            _manufacturerCRUDService.AddNewManufacturer(manufacturer);
            return Ok();
        }

    }
}
