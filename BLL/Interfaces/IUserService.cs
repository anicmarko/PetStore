using BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<RegistrationResponse> RegisterUser(RegisterUserDTO dto);
        Task<LoginResponse> LoginUser(LoginDTO dto);

    }
}
