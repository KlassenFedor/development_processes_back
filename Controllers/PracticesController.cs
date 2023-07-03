using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Practices.RequestModels;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

public class PracticesController : BaseController
{
    private readonly PracticesService _practicesService;

    public PracticesController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _practicesService = serviceProvider.GetRequiredService<PracticesService>();
    }

    [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
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

    [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
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

    [Authorize]
    [HttpPost("{userId:guid}/add_practice")]
    public async Task<IActionResult> AddPractice(Guid? userId, [FromBody] AddPracticeRequest model)
    {
        try
        {
            var result = await _practicesService.AddPractice(userId, model);
            return Ok(result);
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

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPractice(Guid? id)
    {
        try
        {
            var result = await _practicesService.GetPractice(id);
            return Ok(result);
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

    [Authorize]
    [HttpPost("{practiceId:guid}/add_practice_diary")]
    public async Task<IActionResult> AddPracticeDiary(Guid? practiceId, [FromForm] AddPracticeDiaryRequest model)
    {
        try
        {
            await _practicesService.AddPracticeDiary(practiceId, model);
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

    [Authorize]
    [HttpGet("{userId:guid}/all_practices")]
    public async Task<IActionResult> GetAllPractices(Guid? userId)
    {
        try
        {
            var result = await _practicesService.GetStudentPractices(userId);
            return Ok(result);
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