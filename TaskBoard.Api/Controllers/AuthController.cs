using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs;
using TaskBoard.Application.Interfaces;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IWebHostEnvironment _env;


    public AuthController(IAuthService authService, IWebHostEnvironment env)
    {
        _authService = authService;
        _env = env;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), 201)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return StatusCode(201, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        
        Response.Cookies.Append("jwt", response.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = !_env.IsDevelopment(),
            SameSite =  SameSiteMode.Strict,
            Expires = DateTimeOffset.Now.AddMinutes(30)
        });
        
        return Ok(new { response.Username, response.UserID });
    }
    
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Append("jwt", "", new CookieOptions
        {
            HttpOnly = true,
            Secure = !_env.IsDevelopment(),
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.Now.AddDays(-1) // past datetime = deletion
        });

        return Ok(new { message = "Déconnexion réussie." });
    }
}