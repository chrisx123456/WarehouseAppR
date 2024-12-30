using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.Models.DTO;
using WarehouseAppR.Server.Services.Interfaces;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }
        [HttpGet("name/{name}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductByName([FromRoute]string name)
        {
            var products = await _productService.GetProductsByName(name);
            return Ok(products);
        }
        
        [HttpGet("{ean}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<ProductDTO>> GetProductByEan([FromRoute]string ean)
        {
            var product = await _productService.GetProductByEan(ean);
            return Ok(product);
        }
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> AddNewProduct([FromBody] ProductDTO productDto)
        {
            await _productService.AddNewProduct(productDto);
            return Ok();
        }
        [HttpPatch("{ean}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> UpdateProductPrice([FromRoute]string ean, [FromQuery]decimal newPrice)
        {
            await _productService.UpdateProductPrice(ean, newPrice);
            return Ok();
        }
        [HttpDelete("{ean}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> DeleteProduct([FromRoute]string ean)
        {
            await _productService.DeleteProductByEan(ean);
            return Ok();
        }
        [HttpPatch("description/{ean}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> UpdateDescription([FromBody] DescriptionDTO desc, [FromRoute]string ean)
        {
            await _productService.UpdateDescription(ean, desc.Description);
            return Ok();
        }

    }
}
