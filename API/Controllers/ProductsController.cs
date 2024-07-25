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
        
        private readonly IValidator<CreateUpdateProductDTO> _productValidator;

        private readonly int _defaultTopN;

        public ProductsController(IProductServices productService, IValidator<CreateUpdateProductDTO> validator, IConfiguration configuration)
        {
            _productService = productService;
            _productValidator = validator;
            _defaultTopN = configuration.GetValue("DefaultTopN", 10);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProductDTO dto)
        {
            try
            {
                var validationResult = _productValidator.Validate(dto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                var newProduct = await _productService.CreateProduct(dto);
                return Ok(newProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        public async Task<ActionResult<List<ProductEntity>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductEntity>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateUpdateProductDTO dto)
        {
            try
            {
                var validationResult = _productValidator.Validate(dto);

                var updated = await _productService.UpdateProduct(id, dto);

                if (!updated)
                {
                    return NotFound();
                }

                return Ok("Product updated successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deleted = await _productService.DeleteProduct(id);

                if (!deleted)
                {
                    return NotFound();
                }

                return Ok("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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
