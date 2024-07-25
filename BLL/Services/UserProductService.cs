﻿using BLL.DTOs;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserProductService : IUserProductService
    {
        private readonly IUserProductRepository _userProductRepository;

        public UserProductService(IUserProductRepository userProductRepository)
        {
            _userProductRepository = userProductRepository;
        }
        public async Task<AddRelationshipResponse> AddProductToUserAsync(ProductToUserDTO dto)
        {
           bool isCreated=  await _userProductRepository.CreateAsync(dto.UserId, dto.ProductId);

           if(!isCreated)
            return new AddRelationshipResponse(false,"Product not added to user");

           return new AddRelationshipResponse(true,"Added succesfully");
        }


        public async Task<List<ProductToUserDTO>> GetProductsByUserAsync(Guid userId)
        {
           var products =  await _userProductRepository.GetProductsByUser(userId);
           
            return new List<ProductToUserDTO>(products.Select(p => new ProductToUserDTO { UserId = userId, ProductId = p.Id }));
        }


        public async Task<bool> RemoveProductFromUserAsync(Guid userId, int productId)
        {
        return await _userProductRepository.DeleteAsync(userId, productId);
        }
    }
}