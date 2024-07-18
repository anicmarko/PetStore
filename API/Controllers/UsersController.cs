using API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _user;

        public UsersController(IUser user)
        {
            _user = user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginUserAsync(LoginDTO dto)
        {
            var response = await _user.LoginUserAsync(dto);

            if (response == null)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterUserAsync(RegisterUserDTO dto)
        {
            var response = await _user.RegisterUserAsync(dto);

            if (response == null)
                return BadRequest(response);

            return Ok(response);
        }

    }
}
