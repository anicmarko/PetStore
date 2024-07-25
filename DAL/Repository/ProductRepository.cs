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
            var existingProduct = GetByIdAsync(product.OwnerId, product.Id);

            if (existingProduct == null)
            {
                return false; // Return false if the product is not found
            }

            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalProducts()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<decimal> GetAveragePrice()
        {
            return await _context.Products.AverageAsync(p => p.Price);
        }

        public async Task<decimal> GetMinPrice()
        {
            return await _context.Products.MinAsync(p => p.Price);
        }

        public async Task<decimal> GetMaxPrice()
        {
            return await _context.Products.MaxAsync(p => p.Price);
        }

        public async Task<int> GetTotalAssignments()
        {
            return await _context.Users.SelectMany(user => user.Products).CountAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductEntity>> GetPopularProducts(int topN)
        {
            return await _context.Products
                .Include(p => p.Users) // Include the Users who have been assigned the product
                .Where(p => p.Users.Any()) // Exclude products with no assignments
                .OrderByDescending(p => p.Users.Count) // Sort by the number of assignments
                .Take(topN) // Limit the number of results
                .ToListAsync();
        }



    }
}
