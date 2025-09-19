using ClientPortalApi.Models;

namespace ClientPortalApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        DateTime ExpiresAt { get; }
    }
}
