using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.DTOs;
using BLL.Interfaces;
using BLL.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginUserAsync(LoginDTO dto)
        {
            var response = await _userService.LoginUser(dto);

            if (!response.Flag)
            {
                return BadRequest(response.Message);
            }

            return Ok(new { Token = response.Token, Message = response.Message });
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterUserAsync(RegisterUserDTO dto)
        {
            var response = await _userService.RegisterUser(dto);

            if (!response.Flag)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Message);
        }

    }
}
