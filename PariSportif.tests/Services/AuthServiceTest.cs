namespace PariSportif.tests;

using Moq;
using PariSportif.Models;
using PariSportif.Dto;
using PariSportif.Repositories;
using PariSportif.Services;
using FluentAssertions;
using PariSportif.Exceptions;
using Microsoft.AspNetCore.Identity.Data;

public class AuthServiceTest
{
    private readonly List<PariSportif.Models.User> users = new()
    {
        new PariSportif.Models.User
        {
            Name = "Dorian",
            Email = "Dorian@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("pieddansletombeau"),
            Cash = 0,
            Freebet = 10
        }
    };

    // -----------------------------
    // Login
    // -----------------------------
    [Fact]
    public async Task TestLogin()
    {
        var repoMock = new Mock<IUserRepository>();
        var jwtMock = new Mock<IJwtService>();

        repoMock.Setup(r => r.FindByEmail("Dorian@gmail.com"))
                .ReturnsAsync(users[0]);
        
        jwtMock.Setup(j => j.GenerateToken(users[0]))
                .ReturnsAsync("fake-jwt-token");

        var service = new AuthService(repoMock.Object, jwtMock.Object);

        var request = new PariSportif.Dto.LoginRequest
        {
            Email = "Dorian@gmail.com",
            Password = "pieddansletombeau"
        };

        var result = await service.Login(request);

        result.Should().Be("fake-jwt-token");
    }
    
    [Fact]
    public async Task TestLoginInvalidEmail()
    {
        var repoMock = new Mock<IUserRepository>();
        var jwtMock = new Mock<IJwtService>();

        repoMock.Setup(r => r.FindByEmail("Dorian@gmail.com"))
                .ReturnsAsync(users[0]);
        
        jwtMock.Setup(j => j.GenerateToken(users[0]))
                .ReturnsAsync("fake-jwt-token");

        var service = new AuthService(repoMock.Object, jwtMock.Object);

        var request = new PariSportif.Dto.LoginRequest
        {
            Email = "ian@gmail.com",
            Password = "pieddansletombeau"
        };

        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => service.Login(request)
        );

        Assert.Equal("Invalid email", ex.Message);
    }

    [Fact]
    public async Task TestLoginInvalidPassword()
    {
        var repoMock = new Mock<IUserRepository>();
        var jwtMock = new Mock<IJwtService>();

        repoMock.Setup(r => r.FindByEmail("Dorian@gmail.com"))
                .ReturnsAsync(users[0]);
        
        jwtMock.Setup(j => j.GenerateToken(users[0]))
                .ReturnsAsync("fake-jwt-token");

        var service = new AuthService(repoMock.Object, jwtMock.Object);

        var request = new PariSportif.Dto.LoginRequest
        {
            Email = "Dorian@gmail.com",
            Password = "pieddansletomb"
        };

        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => service.Login(request)
        );

        Assert.Equal("Invalid password", ex.Message);
    }

    // -----------------------------
    // Resgister
    // -----------------------------

    [Fact]
    public async Task TestRegister()
    {
        var repoMock = new Mock<IUserRepository>();
        var jwtMock = new Mock<IJwtService>();

        var request = new PariSportif.Dto.RegisterRequest
        {
            Name = "Thomas",
            Email = "thomas@test.com",
            Password = "Password123!"
        };

        repoMock.Setup(r => r.FindByEmail(request.Email))
                .ReturnsAsync((User)null);

        repoMock.Setup(r => r.Create(It.IsAny<User>()))
                .Returns(Task.CompletedTask);


        var service = new AuthService(repoMock.Object, jwtMock.Object);

        var result = await service.Register(request);

        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal(0, result.Cash);
        Assert.Equal(10, result.Freebet);
    }

    [Fact]
    public async Task TestRegisterWithExistingUser()
    {
        var repoMock = new Mock<IUserRepository>();
        var jwtMock = new Mock<IJwtService>();

        var request = new PariSportif.Dto.RegisterRequest
        {
            Name = "Dorian",
            Email = "Dorian@gmail.com",
            Password = "Password123!"
        };

        repoMock.Setup(r => r.FindByEmail(request.Email))
                .ReturnsAsync(new User
                {
                    Email = request.Email
                });

        var service = new AuthService(repoMock.Object, jwtMock.Object);

        var ex = await Assert.ThrowsAsync<UserFoundException>(
            () => service.Register(request)
        );

        Assert.Equal($"{request.Email} already exist", ex.Message);
    }
}