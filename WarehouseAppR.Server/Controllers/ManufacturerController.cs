using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturersService _manufacturerService;
        public ManufacturerController(IManufacturersService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }
        [HttpGet]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<IEnumerable<ManufacturerDTO>>> GetAll()
        {
            return Ok(await _manufacturerService.GetAllManufacturers());
        }
        [HttpGet("{name}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<ManufacturerDTO>> GetByName([FromRoute] string name)
        {
            return Ok(await _manufacturerService.GetManufacturerByName(name));
        }
        [HttpDelete("{name}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> Delete([FromRoute] string name)
        {
            await _manufacturerService.DeleteManufacturerByName(name);
            return NoContent();
        }
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> AddNew([FromBody] ManufacturerDTO manufacturer)
        {
            await _manufacturerService.AddNewManufacturer(manufacturer);
            return Ok();
        }
        [HttpPatch("{name}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> RenameManufacturer([FromRoute]string name, [FromQuery]string newName)
        {
            await _manufacturerService.UpdateManufacturerName(name, newName);
            return Ok();
        }

    }
}
