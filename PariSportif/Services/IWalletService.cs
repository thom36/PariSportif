using PariSportif.Models;
using PariSportif.Dto;

namespace PariSportif.Services
{
    public interface IWalletService
    {
        public Task Deposit(int userId, double amount);
        public Task Withdraw(int userId, double amount);
        public  Task<double> GetBalance(int userId);
        
        // Bet
    }
}