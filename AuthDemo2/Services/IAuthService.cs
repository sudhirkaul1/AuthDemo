using AuthDemo2.Models;

namespace AuthDemo2.Services
{
    public interface IAuthService
    {
        Task<bool> Register(LoggedUser User);
        Task<bool> Login(LoggedUser user);
        Task<string> GenerateToken(LoggedUser user);
    }
}