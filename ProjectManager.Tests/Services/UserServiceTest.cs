using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using ProjectManager.API.Application.DTOs;
using ProjectManager.API.Application.Interfaces;
using ProjectManager.API.Application.Services;
using ProjectManager.API.Domain.Entities;

namespace ProjectManager.Tests.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _configurationMock = new Mock<IConfiguration>();
            
            _userService = new UserService(_userRepositoryMock.Object, _configurationMock.Object );
        }

        [Fact] // Indica que es una prueba unitaria
    public async Task RegisterUserAsync_ShouldReturnFalse_WhenUserAlreadyExists()
    {
        // Arrange: Simulate the existingUser 
       
        var existingUser = new User { 
            Id = 1, 
            Username = "testuser", 
            FirstName = "Test", 
            LastName = "User" 
        };
        
        //Simulate the search of the user and the return
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync("testuser")).ReturnsAsync(existingUser);

        //Arrange: Simulate RegisterDTO
        var registerDto = new RegisterUserDto { 
            Username = "testuser", 
            FirstName = "Test", 
            LastName = "User", 
            Password = "Test123!" 
        };
        
        // Act: We try to register an user that already exist
        var result = await _userService.RegisterUserAsync(registerDto);
        

        // Assert: Verify the result is false
        Assert.False(result);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldReturnTrue_WhenUserDoNotExists()
    {
        _userRepositoryMock.Setup(repo => repo.GetByUsernameAsync("newUser")).ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

        var registerDto = new RegisterUserDto
        {
            Username = "newuser",
            FirstName = "New",
            LastName = "User",
            Password = "SecurePassword123!"
        };

        var result = await _userService.RegisterUserAsync(registerDto);
        Assert.True(result);
    }
    
    [Fact]
    public async Task RegisterUserAsync_ShouldReturnFalse_WhenPasswordIsEmpty()
    {
        var registerDto = new RegisterUserDto
        {
            Username = "",
            FirstName = "New",
            LastName = "User",
            Password = ""
        };

        var result = await _userService.RegisterUserAsync(registerDto);

        Assert.False(result);
    }

    

    

    }
}