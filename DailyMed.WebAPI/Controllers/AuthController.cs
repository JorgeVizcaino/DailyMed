using DailyMed.Core.Interfaces;
using DailyMed.Core.Models.User;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DailyMed.Core.Applications;

namespace DailyMed.WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginUserApplication _userRepo;
        private readonly string _jwtSecretKey;

        public AuthController(ILoginUserApplication userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _jwtSecretKey = config.GetValue<string>("JwtSettings:SecretKey");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Core.Models.User.RegisterRequest request)
        {
            // 1) check if user already exists
            var existing = await _userRepo.GetByUsernameAsync(request.Username);
            if (existing != null)
            {
                return BadRequest("Username already exists.");
            }

            // 2) hash password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3) create user
            var user = new LoginUser
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = request.Role ?? "User"
            };
            int newUserId = await _userRepo.CreateUserAsync(user);

            return Ok(new { Message = $"User created with Id {newUserId}" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Core.Models.User.LoginRequest request)
        {
            var user = await _userRepo.GetByUsernameAsync(request.Username);
            if (user == null)
                return Unauthorized("Invalid credentials");

            bool validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!validPassword)
                return Unauthorized("Invalid credentials");

            var token = GenerateJwtToken(user);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(LoginUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
