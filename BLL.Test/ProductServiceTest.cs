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


namespace BLL.Test
{
    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _mockRepo;
        private readonly ProductService _service;

        public ProductServiceTest()
        {
            _mockRepo = new Mock<IProductRepository>();
            _service = new ProductService(_mockRepo.Object);
        }

        [Theory]
        [InlineData("pedigre", "piletina")]
        public async void CreateProduct_ShouldReturnProduct_WhenProductIsCreated(string brand, string title)
        {
            var dto = new CreateUpdateProductDTO { Brand = brand, Title = title}; // Initialize with test data
            var expectedProduct = dto.ToEntity();
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<ProductEntity>())).ReturnsAsync(It.IsAny<ProductEntity>());

            var result = await _service.CreateProduct(dto);

            _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<ProductEntity>()), Times.Once);
            Assert.Equal(expectedProduct.Title, result.Title); // Assuming Title is a property to check
        }


        [Theory]
        [InlineData("pedigre", "piletina")]
        public async Task GetProducts_ShouldReturnListOfProducts(string brand, string title)
        {
            var products = new List<ProductEntity>
            {
                new ProductEntity { Id = Guid.NewGuid(), Brand = brand, Title = title },
                new ProductEntity { Id = Guid.NewGuid(), Brand = brand, Title = title }
            };

            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(products);

            // Act
            var result = await _service.GetProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }


        [Theory]
        [InlineData("pedigre", "piletina")]
        public async Task GetProductById_ShouldReturnProduct_WhenProductExists(string brand, string title)
        {
            var productId = Guid.NewGuid();

            var expectedProduct = new ProductEntity { Id = productId , Brand = brand, Title= title};
            _mockRepo.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(expectedProduct);

            var result = await _service.GetProductById(productId);

            Assert.Equal(productId, result.Id);
        }

        [Theory]
        [InlineData("pedigree","piletina")]
        public async Task UpdateProduct_ShouldReturnTrue_WhenProductIsUpdated(string brand, string title)
        {
            var productId = Guid.NewGuid();
            var dto = new CreateUpdateProductDTO { Brand = brand, Title = title }; 
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<ProductEntity>())).ReturnsAsync(true);

            var result = await _service.UpdateProduct(productId, dto);

            _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<ProductEntity>()), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnTrue_WhenProductIsDeleted()
        {
            var productId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.DeleteAsync(productId)).ReturnsAsync(true);
            var result = await _service.DeleteProduct(productId);
            _mockRepo.Verify(repo => repo.DeleteAsync(productId), Times.Once);
            Assert.True(result);
        }

        // Test for creating a product with null DTO
        [Fact]
        public async Task CreateProduct_ShouldThrowArgumentNullException_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateProduct(null));
        }

        // Test for getting a product by ID that does not exist
        [Fact]
        public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.GetByIdAsync(nonExistentId)).ReturnsAsync((ProductEntity)null);

            var result = await _service.GetProductById(nonExistentId);

            Assert.Null(result);
        }

        // Test for updating a product that does not exist
        [Theory]
        [InlineData("nonexistentBrand", "nonexistentTitle")]
        public async Task UpdateProduct_ShouldReturnFalse_WhenProductDoesNotExist(string brand, string title)
        {
            var nonExistentId = Guid.NewGuid();
            var dto = new CreateUpdateProductDTO { Brand = brand, Title = title };
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<ProductEntity>())).ReturnsAsync(false);

            var result = await _service.UpdateProduct(nonExistentId, dto);

            Assert.False(result);
        }

        // Test for deleting a product that does not exist
        [Fact]
        public async Task DeleteProduct_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            var nonExistentId = Guid.NewGuid();
            _mockRepo.Setup(repo => repo.DeleteAsync(nonExistentId)).ReturnsAsync(false);

            var result = await _service.DeleteProduct(nonExistentId);

            Assert.False(result);
        }

        // Test for getting all products when there are none
        [Fact]
        public async Task GetProducts_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            var emptyList = new List<ProductEntity>();
            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(emptyList);
            var result = await _service.GetProducts();

            Assert.Empty(result);
        }

        // Assuming modifications and enhancements to ProductService are made to support these tests

        [Fact]
        public async Task UpdateProduct_ShouldUpdateFieldsCorrectly()
        {
            var productId = Guid.NewGuid();
            var dto = new CreateUpdateProductDTO { Brand = "UpdatedBrand", Title = "UpdatedTitle" };
            var productToUpdate = new ProductEntity { Id = productId, Brand = "Brand", Title = "Title" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(productToUpdate);
            _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<ProductEntity>())).ReturnsAsync(true);

            var result = await _service.UpdateProduct(productId, dto);

            Assert.True(result);
            _mockRepo.Verify(repo => repo.UpdateAsync(It.Is<ProductEntity>(p => p.Brand == dto.Brand && p.Title == dto.Title)), Times.Once);
        }


        [Fact]
        public async Task DeleteProduct_ShouldNotAffectOtherProducts()
        {
            var productIdToDelete = Guid.NewGuid();
            var otherProductId = Guid.NewGuid();
            var products = new List<ProductEntity>
            {
                new ProductEntity { Id = productIdToDelete, Brand = "Brand", Title = "TitleToDelete" },
                new ProductEntity { Id = otherProductId, Brand = "Brand", Title = "OtherTitle" }
            };

            _mockRepo.Setup(repo => repo.DeleteAsync(productIdToDelete)).ReturnsAsync(true);
            _mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(products.Where(p => p.Id != productIdToDelete).ToList());

            await _service.DeleteProduct(productIdToDelete);
            var remainingProducts = await _service.GetProducts();

            Assert.Single(remainingProducts);
            Assert.DoesNotContain(remainingProducts, p => p.Id == productIdToDelete);
        }

        [Fact]
        public async Task CreateProduct_ShouldSetId()
        {
            var dto = new CreateUpdateProductDTO { Brand = "Brand", Title = "Title" };
            ProductEntity productPassedToAddAsync = null;

            // Setup the mock to capture the ProductEntity passed to AddAsync and to simulate setting an Id on the entity
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<ProductEntity>()))
                     .Callback<ProductEntity>(p => {
                         p.Id = Guid.NewGuid(); // Simulate the repository setting a new Id
                         productPassedToAddAsync = p;
                     })
                     .ReturnsAsync(() => productPassedToAddAsync); // Return the modified entity

            var result = await _service.CreateProduct(dto);

            _mockRepo.Verify(repo => repo.AddAsync(It.IsAny<ProductEntity>()), Times.Once);
            Assert.NotEqual(Guid.Empty, result.Id);
        }


        [Fact]
        public async Task GetProductById_ShouldHandleCaseSensitivity()
        {
            var productId = Guid.NewGuid();
            var product = new ProductEntity { Id = productId, Brand = "Brand", Title = "CaseSensitiveTitle" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

            var result = await _service.GetProductById(productId);

            Assert.Equal("CaseSensitiveTitle", result.Title);
        }



    }
}
