using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services;
using Microsoft.Extensions.Configuration;
using DAL.Interfaces;
using Moq;
using NSubstitute;
using BLL.DTOs;
using DAL.Entities;

namespace BLL.Test
{
    public class UserServiceTest
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _configurationMock = new Mock<IConfiguration>();

            _userService = new UserService(_userRepositoryMock.Object, _configurationMock.Object);
            _configurationMock.Setup(c => c["JwtSettings:Key"]).Returns("YXNkZmFzZGZhc2RmYXNkZmFzZGZhc2RmYXNkZmF");
            _configurationMock.Setup(c => c["JwtSettings:Issuer"]).Returns("TestIssuer");
            _configurationMock.Setup(c => c["JwtSettings:Audience"]).Returns("TestAudience");
        }

        [Fact]
        public async Task LoginUser_ValidCredentials_ReturnsSuccess()
        {
            // Arrange
            var loginDTO = new LoginDTO { Email = "test@example.com", Password = "password" };
            var userEntity = new UserEntity { Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password"), Name = "Test User" };
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(loginDTO.Email)).ReturnsAsync(userEntity);

            // Act
            var result = await _userService.LoginUser(loginDTO);

            // Assert
            Assert.True(result.Flag);
            Assert.Equal("Login successful", result.Message);
        }

        

        [Fact]
        public async Task LoginUser_InvalidEmail_ReturnsFailure()
        {
            // Arrange
            var loginDTO = new LoginDTO { Email = "wrong@example.com", Password = "password" };
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(loginDTO.Email)).ReturnsAsync((UserEntity?)null); // Fixed Problem 2

            // Act
            var result = await _userService.LoginUser(loginDTO);

            // Assert
            Assert.False(result.Flag);
            Assert.Equal("Invalid email or password", result.Message);
        }

        [Fact]
        public async Task LoginUser_InvalidPassword_ReturnsFailure()
        {
            // Arrange
            var loginDTO = new LoginDTO { Email = "test@example.com", Password = "wrongpassword" };
            var userEntity = new UserEntity { Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password") , Name="Test user"};
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(loginDTO.Email)).ReturnsAsync(userEntity);

            // Act
            var result = await _userService.LoginUser(loginDTO);

            // Assert
            Assert.False(result.Flag);
            Assert.Equal("Invalid email or password", result.Message);
        }

        [Fact]
        public async Task RegisterUser_SuccessfulRegistration_ReturnsSuccess()
        {
            // Arrange
            var registerDTO = new RegisterUserDTO { Email = "new@example.com", Password = "password", Name = "New User", ConfirmPassword="password" };
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(registerDTO.Email)).ReturnsAsync((UserEntity)null);

            // Act
            var result = await _userService.RegisterUser(registerDTO);

            // Assert
            Assert.True(result.Flag);
            Assert.Equal("User is registered successful", result.Message);
        }

        [Fact]
        public async Task RegisterUser_EmailAlreadyExists_ReturnsFailure()
        {
            // Arrange
            var registerDTO = new RegisterUserDTO { Email = "existing@example.com", Password = "password", Name = "Existing User", ConfirmPassword="password" };
            var userEntity = new UserEntity { Email = "existing@example.com", Name = "Test user", Password="password" };
            _userRepositoryMock.Setup(repo => repo.GetUserByEmailAsync(registerDTO.Email)).ReturnsAsync(userEntity);

            // Act
            var result = await _userService.RegisterUser(registerDTO);

            // Assert
            Assert.False(result.Flag);
            Assert.Equal("Email already exists", result.Message);
        }


    }
}
