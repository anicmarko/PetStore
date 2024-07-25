using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IValidator<LoginDTO> _loginValidator;
        private readonly IValidator<RegisterUserDTO> _registerValidator;


        public UserService(IUserRepository userRepository, IConfiguration configuration, IValidator<LoginDTO> loginValidator, IValidator<RegisterUserDTO> registerValidator)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }


        public async Task<LoginResponse> LoginUser(LoginDTO dto)
        {
            var validationResult = _loginValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return new LoginResponse(false, validationResult.Errors.First().ErrorMessage);
            }

            var getUser = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (getUser == null || !BCrypt.Net.BCrypt.Verify(dto.Password, getUser.Password))
            {
                return new LoginResponse(false, "Invalid email or password");
            }
            return new LoginResponse(true, "Login successful", GenerateJWTToken(getUser));
        }


        public async Task<RegistrationResponse> RegisterUser(RegisterUserDTO dto)
        {
            ValidationResult validationResult = _registerValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return new RegistrationResponse(false, validationResult.Errors.First().ErrorMessage);
            }

            var getUser = await _userRepository.GetUserByEmailAsync(dto.Email!);

            if (getUser != null)
                return new RegistrationResponse(false, "Email already exists");

            await _userRepository.AddUserAsync(new UserEntity
            {
                Name = dto.Name!,
                Email = dto.Email!,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            });

            await _userRepository.SaveChanges();
            return new RegistrationResponse(true, "User is registered successfully");
        }

        private string GenerateJWTToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var token = new JwtSecurityToken(
                _configuration["JwtSettings:Issuer"],
                _configuration["JwtSettings:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
