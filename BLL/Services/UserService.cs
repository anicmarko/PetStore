using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
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

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }


        public async Task<LoginResponse> LoginUser(LoginDTO dto)
        {
            var getUser = await _userRepository.GetUserByEmailAsync(dto.Email);

            if (getUser == null || !BCrypt.Net.BCrypt.Verify(dto.Password, getUser.Password))
            {
                return new LoginResponse(false, "Invalid email or password");
            }
            return new LoginResponse(true, "Login successful", GenerateJWTToken(getUser));

        }


        public async Task<RegistrationResponse> RegisterUser(RegisterUserDTO dto)
        {
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
            return new RegistrationResponse(true, "User is registered successful");
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
