using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductEntity> AddAsync(ProductEntity product)
        {
            await _context.Products.AddAsync(product);
            await SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(Guid userId, int id)
        {
            var product = await GetByIdAsync(userId,id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductEntity>> GetAll(Guid userId)
        {
            return await _context.Products
                .Where(p => p.OwnerId == userId)
                .ToListAsync();
        }

        public async Task<ProductEntity> GetByIdAsync(Guid userId, int id)
        {
            return await _context.Products
                   .Where(p => p.OwnerId == userId && p.Id == id)
                   .FirstOrDefaultAsync()
                   ?? throw new InvalidOperationException("Product not found");

        }


        public async Task<bool> UpdateAsync(ProductEntity product)
        {
            _context.Products.Update(product);
            await SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
