﻿using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs
{
    public class RegisterUserDTO
    {

        
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        
        public required string Password { get; set; }

        [Compare(nameof(Password))]
        public required string ConfirmPassword { get; set; }

    }
}
