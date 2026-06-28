using PariSportif.Models;

namespace PariSportif.Services
{
    public interface IJwtService
    {
        public Task<string> GenerateToken(User user);
    }
}