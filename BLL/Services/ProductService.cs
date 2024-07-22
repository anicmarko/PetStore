using DAL.Context;

using BLL.DTOs;
using DAL.Entities;
using BLL.Interfaces;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using BLL.Extensions;

namespace BLL.Services
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
            ArgumentNullException.ThrowIfNull(dto);

            var newProduct = dto.ToEntity();
            await _productRepository.AddAsync(newProduct);

            return newProduct;
        }

        public async Task<List<ProductEntity>> GetProducts()
        {
            return await _productRepository.GetAll();
        }

        public async Task<ProductEntity> GetProductById(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateProduct(Guid id, CreateUpdateProductDTO dto)
        {
            return await _productRepository.UpdateAsync(dto.ToEntity());
        }

        public async Task<bool> DeleteProduct(Guid id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}
