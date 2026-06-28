using Microsoft.EntityFrameworkCore.Diagnostics;
using PariSportif.Exceptions;
using PariSportif.Repositories;

namespace PariSportif.Services
{
    public class WalletService : IWalletService
    {
        private IUserRepository _userRepo;
        public WalletService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task Deposit(int userId, double amount)
        {
            if(amount <= 0) throw new ArgumentException("Amount must be positive");
            var user = await _userRepo.FindById(userId);
            if(user == null) throw new NotFoundException("User not found");
            user.Cash += amount;
            await _userRepo.Update(user);
        }

        public async Task<double> GetBalance(int userId)
        {
            var user = await _userRepo.FindById(userId);
            if(user == null) throw new NotFoundException("User not found");
            return user.Cash;
        }

        public async Task Withdraw(int userId, double amount)
        {
            if(amount <= 0) throw new ArgumentException("Amount must be positive");
            var user = await _userRepo.FindById(userId);
            if(user == null) throw new NotFoundException("User not found");
            var userBalance = user.Cash;
            if(userBalance < amount) throw new WithdrawException($"Not enough cash to withdraw {amount} €");
            user.Cash -= amount;
        }
    }
}