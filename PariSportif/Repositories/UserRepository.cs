using Microsoft.EntityFrameworkCore;
using PariSportif.Models;

namespace PariSportif.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Create(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _context.Users
                            .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> FindById(int id)
        {
            return await _context.Users
                            .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}