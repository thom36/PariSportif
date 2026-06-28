using Microsoft.AspNetCore.Mvc;
using PariSportif.Dto;
using PariSportif.Exceptions;
using PariSportif.Services;

namespace PariSportif.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.Login(request);
                return Ok(new { token });
            }
            catch (NotFoundException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = await _authService.Register(request);
                return Ok(user);
            }
            catch (UserFoundException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}