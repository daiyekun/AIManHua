using System.Security.Claims;
using AIManHua.ApiService.Dtos;
using AIManHua.Domain.Entities;
using AIManHua.Domain.Interfaces;
using AIManHua.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIManHua.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly JwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUserRepository userRepo, JwtService jwtService, ILogger<AuthController> logger)
    {
        _userRepo = userRepo;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (await _userRepo.EmailExistsAsync(request.Email))
            return Conflict(new { message = "该邮箱已被注册" });

        if (await _userRepo.UsernameExistsAsync(request.Username))
            return Conflict(new { message = "该用户名已被使用" });

        var user = new User
        {
            Email = request.Email.Trim().ToLowerInvariant(),
            Username = request.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userRepo.AddAsync(user);

        var (token, expiresAt) = _jwtService.GenerateToken(user.Id, user.Email, user.Username);

        _logger.LogInformation("New user registered: {Email}, Id={Id}", user.Email, user.Id);

        return Ok(new AuthResponse(user.Id, user.Username, user.Email, token, expiresAt));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userRepo.GetByEmailAsync(request.Email.Trim().ToLowerInvariant());
        if (user is null)
            return Unauthorized(new { message = "邮箱或密码错误" });

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized(new { message = "邮箱或密码错误" });

        user.LastLoginAt = DateTime.UtcNow;
        await _userRepo.UpdateAsync(user);

        var (token, expiresAt) = _jwtService.GenerateToken(user.Id, user.Email, user.Username);

        _logger.LogInformation("User logged in: {Email}", user.Email);

        return Ok(new AuthResponse(user.Id, user.Username, user.Email, token, expiresAt));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim is null || !long.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var user = await _userRepo.GetByIdAsync(userId);
        if (user is null)
            return NotFound();

        return Ok(new { user.Id, user.Username, user.Email, user.CreatedAt, user.LastLoginAt });
    }
}
