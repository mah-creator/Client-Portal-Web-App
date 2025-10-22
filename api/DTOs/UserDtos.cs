using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ClientPortalApi.DTOs;

public record UserDto(string Id, string Email, string Name, string Role);

public record ProfileDto(string Id, string Email, string Name, string Bio, string Phone, string AvatarUrl, DateTime CreatedAt, DateTime UpdatedAt);

public record UpdateProfileDto(string? Name, [Phone(ErrorMessage = "Invalid phone number")] string? Phone, string? Bio);
