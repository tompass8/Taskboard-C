using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs.Workspaces;
using TaskBoard.Application.Interfaces;

namespace TaskBoard.Api.Controllers;

// On protège toute la route : il faut obligatoirement un Token JWT pour passer !
[Authorize] 
[ApiController]
[Route("api/[controller]")]
public class WorkspacesController : ControllerBase
{
    private readonly IWorkspaceService _workspaceService;

    public WorkspacesController(IWorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyWorkspaces()
    {
        // On extrait l'ID de l'utilisateur directement depuis son Token de connexion
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized("Token invalide ou utilisateur introuvable.");

        var workspaces = await _workspaceService.GetUserWorkspacesAsync(userId);
        return Ok(workspaces);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequest request)
    {
        // On vérifie que le formulaire envoyé par le frontend est valide
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized("Token invalide.");

        var newWorkspace = await _workspaceService.CreateWorkspaceAsync(request, userId);
        
        // Retourne un code 201 (Created) avec le nouvel objet
        return CreatedAtAction(nameof(GetMyWorkspaces), new { id = newWorkspace.Id }, newWorkspace);
    }
}