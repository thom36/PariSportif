using PariSportif.Models;

namespace PariSportif.Repositories
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task<User> FindById(int id);
        Task<User> FindByEmail(string email);
        Task Update(User user);
    }
}