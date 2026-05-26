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
        // On renvoie l'objet AuthResponse COMPLET (avec le Token)
        return StatusCode(201, response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        
        // On renvoie l'objet AuthResponse COMPLET (avec le Token)
        // C'est ça qui manquait pour que le Javascript le trouve !
        return Ok(response);
    }
    
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // Avec le système LocalStorage, la déconnexion se gère à 100% côté Javascript
        // en supprimant le token du navigateur. L'API a juste besoin de dire OK.
        return Ok(new { message = "Déconnexion réussie." });
    }
}