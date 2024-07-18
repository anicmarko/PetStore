using API.Context;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Repository
{
    public class UserRepository : IUser
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private async Task<UserEntity?> FindUserByEmail(string email) => await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
       
        public async Task<LoginResponse> LoginUserAsync(LoginDTO dto)
        {
            var getUser = await FindUserByEmail(dto.Email);
            
            if(getUser == null)
                return new LoginResponse(false, "Invalid email");

            
            bool isValidPassword =  BCrypt.Net.BCrypt.Verify(dto.Password, getUser.Password);

            if(!isValidPassword)
                return new LoginResponse(false, "Invalid password");

            return new LoginResponse(true, "Login successful", GenerateJWTToken(getUser));
        }

        private string GenerateJWTToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var token = new JwtSecurityToken(
                _configuration["JWTSettings:Issuer"],
                _configuration["JWTSettings:Audience"],
                claims:userClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RegistrationResponse> RegisterUserAsync(RegisterUserDTO dto)
        {
            var getUser = await FindUserByEmail(dto.Email!);

            if(getUser != null)
                return new RegistrationResponse(false, "Email already exists");

            _context.Users.Add(new UserEntity
            {
                Name = dto.Name!,
                Email = dto.Email!,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            });

            await _context.SaveChangesAsync();

            return new RegistrationResponse(true, "User is registered successful");
        }
    }
}
