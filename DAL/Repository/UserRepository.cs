using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserEntity?> GetUserByEmailAsync(string email) =>
             await _context.Users.FirstOrDefaultAsync(user => user.Email == email);

        public async Task<UserEntity?> AddUserAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
        
    }
}
