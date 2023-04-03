using dev_processes_backend.Exceptions;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

public class UsersController : BaseController
{
    private readonly UsersService _usersService;
    
    public UsersController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
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
}