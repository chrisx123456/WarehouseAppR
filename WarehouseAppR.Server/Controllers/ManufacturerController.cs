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
        public async Task<ActionResult<IEnumerable<ManufacturerDTO>>> GetAll()
        {
            return Ok(await _manufacturerCRUDService.GetAllManufacturers());
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<ManufacturerDTO>> GetByName([FromRoute] string name)
        {
            return Ok(await _manufacturerCRUDService.GetManufacturerByName(name));
        }
        [HttpDelete("{name}")]
        public async Task<ActionResult> Delete([FromRoute] string name)
        {
            await _manufacturerCRUDService.DeleteManufacturerByName(name);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult> AddNew([FromBody] ManufacturerDTO manufacturer)
        {
            await _manufacturerCRUDService.AddNewManufacturer(manufacturer);
            return Ok();
        }

    }
}
