using API.Context;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProductDTO dto)
        {
            try {
                var newProduct = new ProductEntity()
                {
                    Brand = dto.Brand,
                    Title = dto.Title
                };

                await _context.Products.AddAsync(newProduct);
                await _context.SaveChangesAsync();

                return Ok("Product added succesful");
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductEntity>>> GetProducts()
        {
            try {
                var products = await _context.Products.ToListAsync();
                return Ok(products);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductEntity>> GetProductById(int id)
        {
            try {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
           
        }

        [HttpPut]
        [Route("{id}")]

        public async Task<ActionResult> UpdateProduct([FromRoute] long id, [FromBody] CreateUpdateProductDTO dto)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

                if (product == null)
                {
                    return NotFound();
                }

                product.Brand = dto.Brand;
                product.Title = dto.Title;

                await _context.SaveChangesAsync();

                return Ok("Product updated succesful");

            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]

        public async Task<ActionResult> DeleteProduct([FromRoute] long id)
        {
            try {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

                if(product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok("Product deleted succesful");
            } catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);}
        }
    }
}
