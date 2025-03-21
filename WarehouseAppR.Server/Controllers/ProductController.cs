﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseAppR.Server.DataAnnotations;
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
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByName([FromRoute]string name)
        {
            var products = await _productService.GetProductsByName(name);
            return Ok(products);
        }
        [HttpGet("tradename/{name}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<ProductDTO>> GetProductByTradeName([FromRoute]string name)
        {
            var prod = await _productService.GetProductByTradeName(name);
            return Ok(prod);
        }

        [HttpGet("{ean}")]
        [Authorize(Roles = "User,Manager,Admin")]
        public async Task<ActionResult<ProductDTO>> GetProductByEan([FromRoute][Ean]string ean)
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
        [HttpDelete("{ean}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> DeleteProduct([FromRoute][Ean]string ean)
        {
            await _productService.DeleteProductByEan(ean);
            return Ok();
        }
        [HttpPatch("{ean}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductPatchDTO patchData, [FromRoute][Ean]string ean)
        {
            await _productService.UpdateProduct(patchData, ean);
            return Ok();
        }
    }
}
