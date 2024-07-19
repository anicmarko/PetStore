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
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productService;
        
        private readonly IValidator<CreateUpdateProductDTO> _productValidator;

        public ProductsController(IProductServices productService, IValidator<CreateUpdateProductDTO> validator)
        {
            _productService = productService;
            _productValidator = validator;
        }

        [Authorize]
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
        public async Task<ActionResult<ProductEntity>> GetProductById(Guid id)
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

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] CreateUpdateProductDTO dto)
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

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
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
    }
}
