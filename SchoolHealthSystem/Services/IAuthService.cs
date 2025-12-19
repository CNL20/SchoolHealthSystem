using SchoolHealthSystem.DTOs.Auth;

namespace SchoolHealthSystem.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequest request);
        Task CreateUserAsync(CreateUserRequest request);
    }
}