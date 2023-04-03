using dev_processes_backend.Exceptions;
using dev_processes_backend.Models.Dtos.Practices.RequestModels;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

public class PracticesController : BaseController
{
    private readonly PracticesService _practicesService;
    
    public PracticesController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _practicesService = serviceProvider.GetRequiredService<PracticesService>();
    }
    
    // TODO allow for only admins and superadmins
    [HttpPost("{id:guid}/characterization")]
    public async Task<IActionResult> Characterization(Guid? id, AddPracticeCharacterizationRequestModel model)
    {
        try
        {
            await _practicesService.AddPracticeCharacterizationAsync(id, model);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    // TODO allow for only admins and superadmins
    [HttpPatch("{id:guid}/characterization/{mark:int}")]
    public async Task<IActionResult> Characterization(Guid? id, int mark)
    {
        try
        {
            await _practicesService.GradePracticeAsync(id, mark);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException)
        {
            return BadRequest();
        }
    }
}