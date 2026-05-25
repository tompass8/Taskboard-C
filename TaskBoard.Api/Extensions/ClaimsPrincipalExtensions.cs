using System.Security.Claims;
using System.Security;

namespace TaskBoard.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserID(this ClaimsPrincipal principal)
    {
        // 'principal' représente ici l'objet 'User' du contrôleur
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid parsedGuid))
        {
            throw new SecurityException("Identifiant utilisateur manquant ou invalide dans le token JWT.");
        }

        return parsedGuid;
    }
}