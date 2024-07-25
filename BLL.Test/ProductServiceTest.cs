using BLL.DTOs;
using BLL.Services;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NSubstitute;
using BLL.Extensions;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using FluentValidation;


namespace BLL.Test
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly Mock<IHttpContextAccessor> _mockAccessor;
        private readonly ProductService _service;
        private readonly Guid _userId;
        private readonly Mock<IValidator<CreateUpdateProductDTO>> _mockValidator;



        public ProductServiceTest()
        {
            _userId = Guid.NewGuid(); // Initialize _userId first
            _mockRepo = new Mock<IProductRepository>();
            _mockAccessor = new Mock<IHttpContextAccessor>();
            _mockValidator = new Mock<IValidator<CreateUpdateProductDTO>>();

            var context = new DefaultHttpContext();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;

            _mockAccessor.Setup(a => a.HttpContext).Returns(context);

            _service = new ProductService(_mockRepo.Object, _mockAccessor.Object, _mockValidator.Object);
        }

        [Theory]
        [InlineData("pedigre", "piletina",150)]
        public async Task CreateProduct_ShouldReturnProduct_WhenProductIsCreated(string brand, string title, decimal price)
        {
            var dto = new CreateUpdateProductDTO { Brand = brand, Title = title, Price = price };
            var product = new ProductEntity { Brand = brand, Title = title, Price = price, OwnerId = _userId };

            _mockValidator.Setup(v => v.Validate(dto)).Returns(new FluentValidation.Results.ValidationResult());
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<ProductEntity>())).ReturnsAsync(product);

            var result = await _service.CreateProduct(dto);

            Assert.NotNull(result);
            Assert.Equal(brand, result.Brand);
            Assert.Equal(title, result.Title);
            Assert.Equal(price, result.Price);
            Assert.Equal(_userId, result.OwnerId);
        }



        [Theory]
        [InlineData("pedigre", "piletina")]
        public async Task GetProducts_ShouldReturnListOfProducts(string brand, string title)
        {
            var products = new List<ProductEntity>
            {
                new ProductEntity { Id = 1, Brand = brand, Title = title , OwnerId = _userId},
                new ProductEntity { Id = 2, Brand = brand, Title = title , OwnerId = _userId}
            };

            _mockRepo.Setup(repo => repo.GetAll(_userId)).ReturnsAsync(products);

            // Act
            var result = await _service.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, product =>
            {
                Assert.Equal(brand, product.Brand);
                Assert.Equal(title, product.Title);
            });
        }




        [Theory]
        [InlineData("pedigre", "piletina")]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists(string brand, string title)
        {
            var productId = 1;

            var expectedProduct = new ProductEntity { Id = productId , Brand = brand, Title= title, OwnerId = _userId};
            _mockRepo.Setup(repo => repo.GetByIdAsync(_userId,productId)).ReturnsAsync(expectedProduct);

            var result = await _service.GetProductById(productId);

            Assert.Equal(productId, result.Id);
        }

        [Theory]
        [InlineData("pedigree", "piletina")]
        public async Task UpdateProduct_ShouldReturnTrue_WhenProductIsUpdated(string brand, string title)
        {
            var productId = 1;
            var dto = new CreateUpdateProductDTO { Brand = brand, Title = title };
            var existingProduct = new ProductEntity { Id = productId, Brand = "oldBrand", Title = "oldTitle", OwnerId = _userId };

            _mockValidator.Setup(v => v.Validate(dto)).Returns(new FluentValidation.Results.ValidationResult());
            _mockRepo.Setup(repo => repo.GetByIdAsync(_userId, productId)).ReturnsAsync(existingProduct);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<ProductEntity>())).ReturnsAsync(true);

            var result = await _service.UpdateProduct(productId, dto);

            Assert.True(result);
            _mockRepo.Verify(repo => repo.UpdateAsync(It.Is<ProductEntity>(p => p.Id == productId && p.Brand == brand && p.Title == title)), Times.Once);
        }



        [Fact]
        public async Task DeleteProduct_ShouldReturnTrue_WhenProductIsDeleted()
        {
            var productId = 1;
            var existingProduct = new ProductEntity { Id = productId, Brand="Pedigree", Title="Piletina", OwnerId = _userId };

            // Setup the mock to return the existing product when GetByIdAsync is called
            _mockRepo.Setup(repo => repo.GetByIdAsync(_userId, productId)).ReturnsAsync(existingProduct);

            // Setup the mock to return true when DeleteAsync is called with the correct parameters
            _mockRepo.Setup(repo => repo.DeleteAsync(_userId, productId)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteProduct(productId);

            // Assert
            _mockRepo.Verify(repo => repo.GetByIdAsync(_userId, productId), Times.Once);
            _mockRepo.Verify(repo => repo.DeleteAsync(_userId, productId), Times.Once);
            Assert.True(result);
        }


        // Test for creating a product with null DTO
        [Fact]
        public async Task CreateProduct_ShouldThrowNullReferenceException_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() => _service.CreateProduct(null));
        }

        // Test for getting a product by ID that does not exist
        [Fact]
        public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            var nonExistentId = 1;
            _mockRepo.Setup(repo => repo.GetByIdAsync(_userId,nonExistentId)).ReturnsAsync((ProductEntity)null);

            var result = await _service.GetProductById(nonExistentId);

            Assert.Null(result);
        }

        // Test for updating a product that does not exist
        [Theory]
        [InlineData("nonexistentBrand", "nonexistentTitle")]
        public async Task UpdateProduct_ShouldThrowProductNotFoundException_WhenProductDoesNotExist(string brand, string title)
        {
            var nonExistentId = 1;
            var dto = new CreateUpdateProductDTO { Brand = brand, Title = title };

            // Setup the mock to return null when GetByIdAsync is called with the non-existent product ID and _userId
            _mockRepo.Setup(repo => repo.GetByIdAsync(_userId, nonExistentId)).ReturnsAsync((ProductEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateProduct(nonExistentId, dto));

            // Verify that GetByIdAsync was called once
            _mockRepo.Verify(repo => repo.GetByIdAsync(_userId, nonExistentId), Times.Once);
            // Verify that UpdateAsync was never called
            _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<ProductEntity>()), Times.Never);
            // Assert the exception message
            Assert.Equal("Product not found", exception.Message);
        }




        // Test for deleting a product that does not exist
        [Fact]
        public async Task DeleteProduct_ShouldThrowProductNotFoundException_WhenProductDoesNotExist()
        {
            var nonExistentId = 1;

            // Setup the mock to throw ProductNotFoundException when GetByIdAsync is called with the non-existent product ID and _userId
            _mockRepo.Setup(repo => repo.GetByIdAsync(_userId, nonExistentId)).ReturnsAsync((ProductEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _service.DeleteProduct(nonExistentId));

            // Verify that GetByIdAsync was called once
            _mockRepo.Verify(repo => repo.GetByIdAsync(_userId, nonExistentId), Times.Once);
            // Verify that DeleteAsync was never called
            _mockRepo.Verify(repo => repo.DeleteAsync(_userId, nonExistentId), Times.Never);
            // Assert the exception message
            Assert.Equal("Product not found", exception.Message);
        }



        // Test for getting all products when there are none
        [Fact]
        public async Task GetProducts_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            var emptyList = new List<ProductEntity>();
            _mockRepo.Setup(repo => repo.GetAll(_userId)).ReturnsAsync(emptyList);
            var result = await _service.GetProducts();

            Assert.Empty(result);
        }

        // Assuming modifications and enhancements to ProductService are made to support these tests

        [Fact]
        public async Task UpdateProduct_ShouldUpdateFieldsCorrectly()
        {
            var productId = 1;
            var dto = new CreateUpdateProductDTO { Brand = "UpdatedBrand", Title = "UpdatedTitle" };
            var productToUpdate = new ProductEntity { Id = productId, Brand = "Brand", Title = "Title", OwnerId = _userId };

            _mockValidator.Setup(v => v.Validate(dto)).Returns(new FluentValidation.Results.ValidationResult());
            _mockRepo.Setup(repo => repo.GetByIdAsync(_userId, productId)).ReturnsAsync(productToUpdate);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<ProductEntity>())).ReturnsAsync(true);

            var result = await _service.UpdateProduct(productId, dto);

            Assert.True(result);
            _mockRepo.Verify(repo => repo.UpdateAsync(It.Is<ProductEntity>(p => p.Id == productId && p.Brand == dto.Brand && p.Title == dto.Title)), Times.Once);
        }




        [Fact]
        public async Task DeleteProduct_ShouldNotAffectOtherProducts()
        {
            var productIdToDelete = 1;
            var otherProductId = 2;
            var products = new List<ProductEntity>
            {
                new ProductEntity { Id = productIdToDelete, Brand = "Brand", Title = "TitleToDelete", OwnerId = _userId },
                new ProductEntity { Id = otherProductId, Brand = "Brand", Title = "OtherTitle", OwnerId = _userId }
            };

            // Setup the mock to return the existing product when GetByIdAsync is called
            _mockRepo.Setup(repo => repo.GetByIdAsync(_userId, productIdToDelete)).ReturnsAsync(products.First(p => p.Id == productIdToDelete));

            // Setup the mock to return true when DeleteAsync is called with the correct parameters
            _mockRepo.Setup(repo => repo.DeleteAsync(_userId, productIdToDelete)).ReturnsAsync(true);

            // Setup the mock to return the remaining products when GetAll is called
            _mockRepo.Setup(repo => repo.GetAll(_userId)).ReturnsAsync(products.Where(p => p.Id != productIdToDelete).ToList());

            // Act
            await _service.DeleteProduct(productIdToDelete);
            var remainingProducts = await _service.GetProducts();

            // Assert
            Assert.Single(remainingProducts);
            Assert.DoesNotContain(remainingProducts, p => p.Id == productIdToDelete);
            Assert.Contains(remainingProducts, p => p.Id == otherProductId);
        }


        [Fact]
        public async Task CreateProduct_ShouldSetId()
        {
            var dto = new CreateUpdateProductDTO { Brand = "Brand", Title = "Title" };
            ProductEntity productPassedToAddAsync = null;

            _mockValidator.Setup(v => v.Validate(dto)).Returns(new FluentValidation.Results.ValidationResult());
            // Setup the mock to capture the ProductEntity passed to AddAsync and to simulate setting an Id on the entity
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<ProductEntity>()))
                     .Callback<ProductEntity>(p => {
                         p.Id = 1; // Simulate the repository setting a new Id
                         productPassedToAddAsync = p;
                     })
                     .ReturnsAsync(() => productPassedToAddAsync); // Return the modified entity

            var result = await _service.CreateProduct(dto);

            _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<ProductEntity>()), Times.Once);
            Assert.NotEqual(0, result.Id);
        }


        [Fact]
        public async Task GetProductById_ShouldHandleCaseSensitivity()
        {
            var productId = 1;
            var product = new ProductEntity { Id = productId, Brand = "Brand", Title = "CaseSensitiveTitle", OwnerId= _userId };

            _mockRepo.Setup(repo => repo.GetByIdAsync(_userId, productId)).ReturnsAsync(product);

            var result = await _service.GetProductById(productId);

            Assert.NotNull(result);
            Assert.Equal("CaseSensitiveTitle", result.Title, ignoreCase: false);
        }




    }
}
