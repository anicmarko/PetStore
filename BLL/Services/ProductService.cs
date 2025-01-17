﻿using DAL.Context;

using BLL.DTOs;
using DAL.Entities;
using BLL.Interfaces;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using BLL.Extensions;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using FluentValidation;

namespace BLL.Services
{
    public class ProductService : IProductServices
    {
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<CreateUpdateProductDTO> _productValidator;


        public ProductService(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor, IValidator<CreateUpdateProductDTO> productValidator)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _productValidator = productValidator;
        }

        public async Task<ProductEntity> CreateProduct(CreateUpdateProductDTO dto)
        {

            var validationResult = _productValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors.First().ErrorMessage);
            }

            var userId = GetCurrentUserId();

            var newProduct = dto.ToEntity();
            newProduct.OwnerId = userId;

            await _productRepository.AddAsync(newProduct);
            return newProduct;
        }

        public async Task<List<ProductEntity>> GetProducts()
        {
            var userId = GetCurrentUserId();

            return await _productRepository.GetAll(userId);
        }

        public async Task<ProductEntity> GetProductById(int id)
        {
            var userId = GetCurrentUserId();
            return await _productRepository.GetByIdAsync(userId, id);
        }

        public async Task<bool> UpdateProduct(int id, CreateUpdateProductDTO dto)
        {
            var userId = GetCurrentUserId();
            var product = await _productRepository.GetByIdAsync(userId, id);
            if (product == null)
            {
                throw new ValidationException("Product not found");
            }

            // Update product properties
            product.Brand = dto.Brand;
            product.Title = dto.Title;

            // Validate the updated product
            var validationResult = _productValidator.Validate(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            return await _productRepository.UpdateAsync(product);
        }



        public async Task<bool> DeleteProduct(int id)
        {
            var userId = GetCurrentUserId();

            var product = await _productRepository.GetByIdAsync(userId, id);

            if (product == null)
            {
                throw new ValidationException("Product not found");
            }

            if (product.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("You are not the owner of this product");
            }
            return await _productRepository.DeleteAsync(userId, id);
        }

        private Guid GetCurrentUserId()
        {

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            return Guid.Parse(userIdClaim.Value);
        }

        public async Task<ProductStatisticsDTO> GetProductStatistics()
        {
            var totalAssignments = await _productRepository.GetTotalAssignments();
            var maxPrice = await _productRepository.GetMaxPrice();
            var minPrice = await _productRepository.GetMinPrice();
            var avgPrice = await _productRepository.GetAveragePrice();
            var totalProducts = await _productRepository.GetTotalProducts();

            ProductStatisticsDTO dto = new ProductStatisticsDTO
            {
                AveragePrice = avgPrice,
                TotalProducts = totalProducts,
                TotalAssignments = totalAssignments,
                MaxPrice = maxPrice,
                MinPrice = minPrice
            };
            return dto;
        }

        //TODO : take topN from appsettings.json
        public async Task<List<PopularProductDTO>> GetPopularProducts(int topN)
        {
            var popularProducts = await _productRepository.GetPopularProducts(topN);

            return popularProducts
                 .Select(product => new PopularProductDTO
                 {
                     ProductId = product.Id,
                     Name = product.Brand + " " + product.Title,
                     AssignmentCount = product.Users.Count,
                     CreatedByUserName = product.Owner.Name
                 }).ToList();
        

    }
    }
}
