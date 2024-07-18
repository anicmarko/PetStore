using API.DTOs;

namespace API.Interfaces
{
    public interface IUser
    {
        Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO dto);
        Task<LoginResponse> LoginUserAsync(LoginDTO dto);


    }
}
