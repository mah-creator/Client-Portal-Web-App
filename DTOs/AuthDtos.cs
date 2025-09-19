namespace ClientPortalApi.DTOs
{
    public record LoginRequest(string Email, string Password);
    public record AuthResponse(string Token, DateTime ExpiresAt, UserDto User);
    public record UserDto(string Id, string Email, string Name, string Role);
}
