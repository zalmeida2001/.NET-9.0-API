using Microsoft.AspNetCore.Mvc;
using TodoListApi.Models;
using TodoListApi.Services;
using TodoListApi.Repositories;
using System.Threading.Tasks;

namespace TodoListApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IAuthRepository _authRepository;

        public AuthController(AuthService authService, IAuthRepository authRepository)
        {
            _authService = authService;
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var existingUser = await _authRepository.GetUserByUsername(user.Username);
            if (existingUser != null)
                return BadRequest("User already exists.");

            bool success = await _authRepository.Register(user);
            if (!success) return StatusCode(500, "Error registering user.");

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.AuthenticateUser(request.Username, request.Password);
            if (token == null)
                return Unauthorized("Invalid username or password.");

            return Ok(new { Token = token });
        }
    }

    public class LoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
