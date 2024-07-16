using API.Context;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ProductService : IProductServices
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductEntity> CreateProduct(CreateUpdateProductDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Brand) || string.IsNullOrEmpty(dto.Title))
            {
                throw new ArgumentException("Brand and Title are required");
            }

            var newProduct = new ProductEntity()
            {
                Brand = dto.Brand,
                Title = dto.Title
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return newProduct;
        }

        public async Task<List<ProductEntity>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductEntity> GetProductById(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                throw new ArgumentException($"Product with ID {id} not found");
            }

            return product;
        }

        public async Task<bool> UpdateProduct(Guid id, CreateUpdateProductDTO dto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(dto.Brand) || string.IsNullOrEmpty(dto.Title))
            {
                throw new ArgumentException("Brand and Title are required");
            }

            product.Brand = dto.Brand;
            product.Title = dto.Title;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
