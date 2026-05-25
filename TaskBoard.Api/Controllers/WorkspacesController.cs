using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.DTOs.Workspaces;
using TaskBoard.Application.Interfaces;

namespace TaskBoard.Api.Controllers;

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
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        // Extraction propre du Guid depuis le JWT
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized("Token invalide ou utilisateur introuvable.");

        var workspaces = await _workspaceService.GetUserWorkspacesAsync(userId);
        return Ok(workspaces);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkspace([FromBody] CreateWorkspaceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        // Extraction propre du Guid depuis le JWT
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized("Token invalide.");

        var newWorkspace = await _workspaceService.CreateWorkspaceAsync(request, userId);
        
        return CreatedAtAction(nameof(GetMyWorkspaces), new { id = newWorkspace.ID }, newWorkspace);
    }
}