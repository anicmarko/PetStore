using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace DAL.Repository
{
    public class UserProductRepository : IUserProductRepository
    {
        private readonly AppDbContext _context;

        public UserProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Guid userId, int productId)
        {
            var user = await _context.Users.FindAsync(userId);
            var product = await _context.Products.FindAsync(productId);

            if (user == null || product == null)
                throw new InvalidOperationException("User or product not found");

            user.Products.Add(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid userId, int productId)
        {
            var user = await _context.Users.Include(u => u.Products).FirstOrDefaultAsync(u => u.Id == userId);
            var product = user?.Products.FirstOrDefault(p => p.Id == productId);


            if (user == null || product == null)
                throw new InvalidOperationException("User or product not found");

            user.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<ProductEntity>> GetProductsByUser(Guid userId)
        {
            var user = await _context.Users.Include(u => u.Products).FirstOrDefaultAsync(user => user.Id == userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            return user.Products.ToList();
        }

    }
}
