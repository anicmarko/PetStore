using API.Context;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ProductService : IProductServices
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductEntity> CreateProduct(CreateUpdateProductDTO dto)
        {

            var newProduct = new ProductEntity()
            {
                Brand = dto.Brand,
                Title = dto.Title
            };

            await _productRepository.AddAsync(newProduct);

            return newProduct;
        }

        public async Task<List<ProductEntity>> GetProducts()
        {
            return await _productRepository.GetAll().ToListAsync();
        }

        public async Task<ProductEntity> GetProductById(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateProduct(Guid id, CreateUpdateProductDTO dto)
        {
            return await _productRepository.UpdateAsync(new ProductEntity()
            {
                Brand = dto.Brand,
                Title = dto.Title
            });
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}
