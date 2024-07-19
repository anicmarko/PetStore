using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class LoginDTO
    {
        [EmailAddress]
        public required string  Email { get; set; }

        public required string Password { get; set; }
    }
}
