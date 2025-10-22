using System.ComponentModel.DataAnnotations;

namespace ClientPortalApi.DTOs;

public record UserDto(string Id, string Email, string Name, string Role);

public record UpdateProfileDto(string Name, [Phone(ErrorMessage = "Invalid phone number")] string Phone, string Bio);
