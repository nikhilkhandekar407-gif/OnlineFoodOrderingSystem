using FoodOrderingAPI.Data;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;
using FoodOrderingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == dto.Email))
                return BadRequest(new { message = "Email is already registered" });

            var customer = new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone,
                Address = dto.Address
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var (token, expiresAt) = _tokenService.CreateToken(customer);

            return Ok(new AuthResponseDto
            {
                CustomerId = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Token = token,
                ExpiresAt = expiresAt
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == dto.Email);
            if (customer == null || !BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password" });

            var (token, expiresAt) = _tokenService.CreateToken(customer);

            return Ok(new AuthResponseDto
            {
                CustomerId = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Token = token,
                ExpiresAt = expiresAt
            });
        }
    }
}
