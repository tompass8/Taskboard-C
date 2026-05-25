using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskBoard.Application.DTOs;
using TaskBoard.Application.Interfaces;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Interfaces;

namespace TaskBoard.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher _passwordHasher;
    
    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        // Using HmacSha256 for fast generation (<1ms)
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            // ClaimTypes est une classe de constantes qui standardise les noms des claims :
            // ClaimTypes.NameIdentifier  →  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            // ClaimTypes.Email           →  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
            // ClaimTypes.Name            →  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
            // Sans ClaimTypes tu devrais écrire ces longues chaînes à la main.
            // Les claims sont lisibles dans le payload (pas chiffrés)
            // Ne JAMAIS y mettre de mot de passe ou données sensibles.
            // Seule la signature garantit que le token est authentique.

            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
        };

        var token = new JwtSecurityToken(

            // Qui a émis le token → doit correspondre à Jwt:Issuer dans appsettings.json
            // Vérifié à chaque requête par JwtBearer
            issuer: _configuration["Jwt:Issuer"],

            // Pour qui est le token → doit correspondre à Jwt:Audience dans appsettings.json
            // Évite qu'un token prévu pour une app soit utilisé sur une autre
            audience: _configuration["Jwt:Audience"],

            // Les informations sur l'utilisateur stockées dans le token
            claims: claims,

            // (optionnel) Date à partir de laquelle le token est valide
            // Utile pour créer un token "en avance" → non utilisé ici
            notBefore: DateTime.UtcNow,

            // Date d'expiration du token → après cette date, le token est rejeté
            // Ici 30 minutes après la création
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:AccessTokenExpiryInMinutes"]!)),

            // La signature cryptographique avec HmacSha256 + la clé secrète
            // Garantit que le token n'a pas été falsifié
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public AuthService(IUserRepository userRepo, IConfiguration configuration, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepo;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.ExistsUserByEmailAsync(request.Email))
            throw new InvalidOperationException("Email already in use");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password)
        };

        var created = await _userRepository.CreateUserAsync(user);
        return new AuthResponse(GenerateToken(created), created.Username, created.ID);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email)
            ?? throw new UnauthorizedAccessException("Invalid Email or Password");
        
        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid Email or Password");
        
        return new AuthResponse(GenerateToken(user), user.Username, user.ID);
    }
}