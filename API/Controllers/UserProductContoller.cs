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
            var response = await _userProductService.AddProductToUserAsync(dto);
            return Ok(response);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetProductsByUser(Guid userId)
        {
            var products = await _userProductService.GetProductsByUserAsync(userId);
            return Ok(products);
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<ActionResult> RemoveProductFromUser(Guid userId, int productId)
        {
            bool isDeleted = await _userProductService.RemoveProductFromUserAsync(userId, productId);
            if (!isDeleted)
            {
                return NotFound("Product not found or not associated with the user.");
            }
            return Ok("Successfully removed");
        }



    }
}
