using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace TaskBoard.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == 401)
                await WriteErrorAsync(context, 401, "Vous n'êtes pas authentifié. Veuillez vous connecter.");
            else if (context.Response.StatusCode == 403)
                await WriteErrorAsync(context, 403, "Vous n'avez pas les droits pour accéder à cette ressource.");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogWarning(ex, "Argument nul");
            await WriteErrorAsync(context, 400, $"{ex.ParamName} ne peut pas être nul.");
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Argument invalide");
            await WriteErrorAsync(context, 400, ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Ressource introuvable");
            await WriteErrorAsync(context, 404, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Accès non autorisé");
            await WriteErrorAsync(context, 401, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Opération invalide");
            await WriteErrorAsync(context, 409, ex.Message);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erreur de mise à jour de la base de données");
            await WriteErrorAsync(context, 409, "Un conflit est survenu lors de l'accès à la base de données.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur inattendue");
            await WriteErrorAsync(context, 500, "Une erreur inattendue est survenue.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            statusCode,
            message,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}