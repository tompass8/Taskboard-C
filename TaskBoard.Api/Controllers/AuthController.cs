using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs;
using TaskBoard.Application.Interfaces;

namespace TaskBoard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // Créer un nouveau compte utilisateur
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), 201)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return StatusCode(201, response);
    }

    // Se connecter et récupérer un token JWT
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return StatusCode(200, response);
    }
}