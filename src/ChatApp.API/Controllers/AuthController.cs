using BCrypt.Net;
using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces;
using ChatApp.Shared.DTOs;
using ChatApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService _jwt;

    public AuthController(IUserRepository userRepo, IJwtService jwt)
    {
        _userRepo = userRepo;
        _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("Username and password are required.");

        if (await _userRepo.ExistsAsync(req.Username))
            return Conflict("Username is already taken.");

        var user = new User
        {
            Username = req.Username.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };

        await _userRepo.AddAsync(user);
        return Ok(new { message = "Account created. You can now log in." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _userRepo.GetByUsernameAsync(req.Username);
        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized("Invalid username or password.");

        var token = _jwt.GenerateToken(user);
        return Ok(new LoginResponse
        {
            Token = token,
            Username = user.Username,
            UserId = user.Id
        });
    }
}
