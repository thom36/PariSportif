using PariSportif.Models;
using PariSportif.Dto;

namespace PariSportif.Services
{
    public interface IAuthService
    {
        public Task<User> Register(RegisterRequest request);
        public Task<string> Login(LoginRequest request);
    }
}