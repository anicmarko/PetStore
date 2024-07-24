using BLL.DTOs;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserProductContoller : ControllerBase
    {
        private readonly IUserProductService _userProductService;

        public UserProductContoller(IUserProductService userProductService)
        {
            _userProductService = userProductService;
        }

        [HttpPost]
        public async Task<ActionResult<AddRelationshipResponse>> AddProductToUser([FromBody] ProductToUserDTO dto)
        {
            try
            {
                var response = await _userProductService.AddProductToUserAsync(dto);
                return Ok(response);
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

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetProductsByUser(Guid userId)
        {
            try
            {
                var products = await _userProductService.GetProductsByUserAsync(userId);
                return Ok(products);
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



        [HttpDelete("{userId}/{productId}")]
        public async Task<ActionResult> RemoveProductFromUser(Guid userId, int productId)
        {
            try
            {
                bool isDeleted = await _userProductService.RemoveProductFromUserAsync(userId, productId);
                if(!isDeleted)
                {
                    return NotFound("Product not found or not associated with the user.");
                }
                return Ok("Succesfully removed");
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

      

    }
}
