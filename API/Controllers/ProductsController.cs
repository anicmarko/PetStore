using BLL.DTOs;
using DAL.Entities;
using BLL.Services;
using BLL.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;
        
        private readonly int _defaultTopN;

        public ProductsController(IProductServices productService, IConfiguration configuration)
        {
            _productService = productService;
            _defaultTopN = configuration.GetValue("DefaultTopN", 10);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProductDTO dto)
        {
            var newProduct = await _productService.CreateProduct(dto);
            return Ok(newProduct);
        }



        [HttpGet]
        public async Task<ActionResult<List<ProductEntity>>> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductEntity>> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateUpdateProductDTO dto)
        {
            var updated = await _productService.UpdateProduct(id, dto);

            if (!updated)
            {
                return NotFound();
            }

            return Ok("Product updated successfully");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProduct(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok("Product deleted successfully");
        }


        [Authorize]
        [HttpGet("statistics")]
        public async Task<ActionResult<ProductStatisticsDTO>> GetProductStatistics()
        {
            var statistics = await _productService.GetProductStatistics();
            return Ok(statistics);
        }

        [Authorize]
        [HttpGet("popular")]
        public async Task<ActionResult<List<PopularProductDTO>>> GetPopularProducts([FromQuery] int? topN)
        {
            var numberOfProducts = topN ?? _defaultTopN;
            var popularProducts = await _productService.GetPopularProducts(numberOfProducts);
            return Ok(popularProducts);
        }


    }
}
