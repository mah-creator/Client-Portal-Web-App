using System.ComponentModel.DataAnnotations;

namespace ClientPortalApi.DTOs
{
    public record LoginRequest(string Email, string Password);
    public record SignupRequest([EmailAddress]string Email, string Name, string Password, string Role);
    public record AuthResponse(string Token, DateTime ExpiresAt, UserDto User);
    public record UserDto(string Id, string Email, string Name, string Role);
}
