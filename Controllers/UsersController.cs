using System.Security.Claims;
using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

public class UsersController : BaseController
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UsersService _usersService;
    
    public UsersController(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor) : base(
        serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _usersService = serviceProvider.GetRequiredService<UsersService>();
    }
    
    // TODO allow for only superadmins
    [HttpPatch("{id:guid}/block_admin")]
    public async Task<IActionResult> BlockAdmin(Guid? id)
    {
        try
        {
            await _usersService.BlockAdminAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    // TODO allow for only superadmins
    [HttpPatch("{id:guid}/unblock_admin")]
    public async Task<IActionResult> UnblockAdmin(Guid? id)
    {
        try
        {
            await _usersService.UnblockAdminAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    [Authorize(Roles = RolesNames.SuperAdministrator)]
    [HttpGet("admins")]
    public async Task<IActionResult> GetAdmins()
    {
        try
        {
            return Ok(await _usersService.GetAdminsAsync());
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid? id)
    {
        var requesterIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        if (requesterIdClaim == null)
        {
            return Forbid();
        }
        try
        {
            await _usersService.DeleteUserAsync(id, new Guid(requesterIdClaim!.Value));
            return Ok();
        }
        catch (NoAccessException)
        {
            Console.WriteLine("FORBIDDING");
            return Forbid();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
}