using API.Context;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
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
            await _context.AddAsync(product);
            await SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await GetByIdAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await SaveChangesAsync();
            return true;
        }

        public IQueryable<ProductEntity> GetAll()
        {
            return _context.Products.AsQueryable();
        }

        public async Task<ProductEntity> GetByIdAsync(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id)
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
