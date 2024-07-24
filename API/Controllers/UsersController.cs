using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.DTOs;
using BLL.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _user;

        public UsersController(IUserService user)
        {
            _user = user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginUserAsync(LoginDTO dto)
        {
            var response = await _user.LoginUser(dto);

            if (response == null || !response.Flag)
                return Unauthorized("Authentication failed.");


            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterUserAsync(RegisterUserDTO dto)
        {
            var response = await _user.RegisterUser(dto);

            if (response == null)
                return Unauthorized(response);

            return Ok(response);
        }

    }
}
