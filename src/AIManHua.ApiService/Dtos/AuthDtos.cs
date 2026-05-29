using System.ComponentModel.DataAnnotations;

namespace AIManHua.ApiService.Dtos;

public record RegisterRequest(
    [Required, EmailAddress, MaxLength(256)] string Email,
    [Required, MinLength(2), MaxLength(64)] string Username,
    [Required, MinLength(6), MaxLength(128)] string Password
);

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public record AuthResponse(
    long UserId,
    string Username,
    string Email,
    string AccessToken,
    DateTime ExpiresAt
);
