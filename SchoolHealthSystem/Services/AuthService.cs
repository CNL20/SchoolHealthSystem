using Microsoft.EntityFrameworkCore;
using SchoolHealthSystem.Data;
using SchoolHealthSystem.DTOs.Auth;
using SchoolHealthSystem.Helpers;
using SchoolHealthSystem.Models;

namespace SchoolHealthSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwt;

        public AuthService(AppDbContext context, JwtTokenHelper jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        public async Task CreateUserAsync(CreateUserRequest request)
        {
            if(_context.Users.Any(u => u.Email == request.Email))
                throw new Exception("Email đã được sử dụng!!");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = PasswordHasher.Hash(request.Password),
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Role = request.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if(user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
                throw new Exception("Email hoặc mật khẩu không hợp lệ!!");

            return _jwt.GenerateToken(user);
        }
    }
}