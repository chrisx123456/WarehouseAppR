using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.Interfaces;
using WarehouseAppR.Server.Models;

namespace WarehouseAppR.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> GetAllProducts()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }
        [HttpGet("{ean}")]
        public ActionResult<ProductDTO> GetProductByEan([FromRoute]string ean)
        {
            var product = _productService.GetProductByEan(ean);
            return Ok(product);
        }
        [HttpPost]
        public ActionResult AddNewProduct([FromBody] ProductDTO productDto)
        {
            _productService.AddNewProduct(productDto);
            return Ok();
        }
        [HttpPatch("{ean}")]
        public ActionResult UpdateProductPrice([FromRoute]string ean, [FromQuery]decimal newPrice)
        {
            _productService.UpdateProductPrice(ean, newPrice);
            return Ok();
        }
        [HttpDelete("{ean}")]
        public ActionResult DeleteProduct([FromRoute]string ean)
        {
            _productService.DeleteProductByEan(ean);
            return Ok();
        }
        [HttpPatch("description/{ean}")]
        public ActionResult UpdateDescription([FromBody] DescriptionDTO desc, [FromRoute]string ean)
        {
            _productService.UpdateDescription(ean, desc.Description);
            return Ok();
        }

    }
}
