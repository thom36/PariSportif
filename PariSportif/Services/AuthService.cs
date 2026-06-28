using PariSportif.Dto;
using PariSportif.Exceptions;
using PariSportif.Models;
using PariSportif.Repositories;
using BCrypt.Net;

namespace PariSportif.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;
        public AuthService(IUserRepository userRepo, IJwtService jwtService)
        {
            _jwtService = jwtService;
            _userRepo = userRepo;
        }
        public async Task<string> Login(LoginRequest request)
        {
            var user = await _userRepo.FindByEmail(request.Email);
            if(user == null) throw new NotFoundException("Invalid email");
            var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if(!isValid) throw new NotFoundException("Invalid password");
            var token = await _jwtService.GenerateToken(user);
            return token;
        }

        public async Task<User> Register(RegisterRequest request)
        {
            var existingUser = await _userRepo.FindByEmail(request.Email);
            if(existingUser != null) throw new UserFoundException($"{request.Email} already exist");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Cash = 0,
                Freebet = 10
            };

            await _userRepo.Create(user);
            return user;
        }
    }
}