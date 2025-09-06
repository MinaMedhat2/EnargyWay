using Company.API.Data;
using Company.API.DTOs;
using Company.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Company.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email || u.Username == registerDto.Username))
            {
                return BadRequest("Username or Email is already taken.");
            }

            var passwordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(registerDto.Password)));

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                Role = Role.User // Default role is User
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful" });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var providedPasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(loginDto.Password)));

            if (user.PasswordHash != providedPasswordHash)
            {
                return Unauthorized("Invalid email or password.");
            }

            string roleName;
            // --- بداية التعديل ---
            // أضفنا حالة جديدة لمندوب التوصيل
            switch (user.Role)
            {
                case Role.Admin:
                    roleName = "Admin";
                    break;
                case Role.StoreManager:
                    roleName = "StoreManager";
                    break;
                case Role.Employee: // Role 2
                    roleName = "DeliveryPerson"; // اسم واضح للـ Frontend
                    break;
                case Role.User:
                default:
                    roleName = "User";
                    break;
            }
            // --- نهاية التعديل ---

            return Ok(new
            {
                message = "Login successful",
                userRole = roleName,
                username = user.Username
            });
        }
    }
}
