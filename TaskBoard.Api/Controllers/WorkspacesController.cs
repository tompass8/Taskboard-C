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
        
        // Transforme la chaîne du Token en 'Guid'
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
        
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized("Token invalide.");

        var newWorkspace = await _workspaceService.CreateWorkspaceAsync(request, userId);
        
        return CreatedAtAction(nameof(GetMyWorkspaces), new { id = newWorkspace.ID }, newWorkspace);
    }

    [HttpGet("{workspaceId}/members")]
    public async Task<IActionResult> GetWorkspaceMembers(int workspaceId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized("Token invalide ou utilisateur introuvable.");

        var members = await _workspaceService.GetWorkspaceMembersAsync(workspaceId, userId);
        return Ok(members);
    }

    [HttpPost("{workspaceId}/members")]
    public async Task<IActionResult> AddWorkspaceMember(int workspaceId, [FromBody] CreateWorkspaceMembershipRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid requesterId))
            return Unauthorized("Token invalide ou utilisateur introuvable.");

        var member = await _workspaceService.AddWorkspaceMemberAsync(workspaceId, request, requesterId);
        return CreatedAtAction(nameof(GetWorkspaceMembers), new { workspaceId }, member);
    }

    [HttpPatch("{workspaceId}/members/{memberUserId}/role")]
    public async Task<IActionResult> UpdateWorkspaceMemberRole(int workspaceId, Guid memberUserId, [FromBody] UpdateWorkspaceMembershipRoleRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid requesterId))
            return Unauthorized("Token invalide ou utilisateur introuvable.");

        var updated = await _workspaceService.UpdateWorkspaceMemberRoleAsync(workspaceId, memberUserId, request, requesterId);
        return Ok(updated);
    }

    [HttpDelete("{workspaceId}/members/{memberUserId}")]
    public async Task<IActionResult> RemoveWorkspaceMember(int workspaceId, Guid memberUserId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid requesterId))
            return Unauthorized("Token invalide ou utilisateur introuvable.");

        await _workspaceService.RemoveWorkspaceMemberAsync(workspaceId, memberUserId, requesterId);
        return NoContent();
    }
}