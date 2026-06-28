using Moq;
using PariSportif.Exceptions;
using PariSportif.Models;
using PariSportif.Repositories;
using PariSportif.Services;

public class WalletServiceTest
{
    private readonly List<PariSportif.Models.User> users = new()
    {
        new PariSportif.Models.User
        {
            Name = "Dorian",
            Email = "Dorian@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("pieddansletombeau"),
            Cash = 70,
            Freebet = 10
        }
    };

    // -----------------------------
    // Get Balance
    // -----------------------------
    [Fact]
    public async Task TestGetBalance()
    {
        var repoMock = new Mock<IUserRepository>();

        repoMock.Setup(r => r.FindById(1))
                .ReturnsAsync(users[0]);

        var service = new WalletService(repoMock.Object);

        var result = await service.GetBalance(1);

        Assert.Equal(70, result);
    }

    // -----------------------------
    // Deposit
    // -----------------------------
    [Fact]
    public async Task TestDeposit()
    {
        var repoMock = new Mock<IUserRepository>();

        repoMock.Setup(r => r.FindById(1))
                .ReturnsAsync(users[0]);

        var service = new WalletService(repoMock.Object);

        await service.Deposit(1, 50);

        var result = await service.GetBalance(1);

        Assert.Equal(120, result);
    }

    [Fact]
    public async Task TestDepositInvalidArgument()
    {
        var repoMock = new Mock<IUserRepository>();

        repoMock.Setup(r => r.FindById(1))
                .ReturnsAsync(users[0]);

        var service = new WalletService(repoMock.Object);

        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => service.Deposit(1, -5)
        );

        Assert.Equal("Amount must be positive", ex.Message);
    }

    [Fact]
    public async Task TestDepositInvalidUser()
    {
        var repoMock = new Mock<IUserRepository>();

        repoMock.Setup(r => r.FindById(2))
                .ReturnsAsync((User)null);

        var service = new WalletService(repoMock.Object);

        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => service.Deposit(1, 10)
        );

        Assert.Equal("User not found", ex.Message);
    }

    // -----------------------------
    // Withdraw
    // -----------------------------

    [Fact]
    public async Task TestWithdraw()
    {
        var repoMock = new Mock<IUserRepository>();

        repoMock.Setup(r => r.FindById(1))
                .ReturnsAsync(users[0]);

        var service = new WalletService(repoMock.Object);

        await service.Withdraw(1, 50);

        var result = await service.GetBalance(1);

        Assert.Equal(20, result);
    }

    [Fact]
    public async Task TestWithdrawInvalidArgument()
    {
        var repoMock = new Mock<IUserRepository>();

        repoMock.Setup(r => r.FindById(1))
                .ReturnsAsync(users[0]);

        var service = new WalletService(repoMock.Object);

        var ex = await Assert.ThrowsAsync<ArgumentException>(
            () => service.Withdraw(1, -5)
        );

        Assert.Equal("Amount must be positive", ex.Message);
    }

    [Fact]
    public async Task TestWithdrawInvalidUser()
    {
        var repoMock = new Mock<IUserRepository>();

        repoMock.Setup(r => r.FindById(2))
                .ReturnsAsync((User)null);

        var service = new WalletService(repoMock.Object);

        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => service.Withdraw(1, 10)
        );

        Assert.Equal("User not found", ex.Message);
    }

    [Fact]
    public async Task TestWithdrawNotEnoughMoney()
    {
        var repoMock = new Mock<IUserRepository>();

        repoMock.Setup(r => r.FindById(1))
                .ReturnsAsync(users[0]);

        var service = new WalletService(repoMock.Object);

        var ex = await Assert.ThrowsAsync<WithdrawException>(
            () => service.Withdraw(1, 80)
        );

        Assert.Equal($"Not enough cash to withdraw {80} €", ex.Message);
    }
}