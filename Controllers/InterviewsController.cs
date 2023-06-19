using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Interviews;
using dev_processes_backend.Models.Dtos.Interviews.InterviewRequest;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

public class InterviewsController : BaseController
{
    private readonly InterviewsService _interviewsService;
    
    public InterviewsController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _interviewsService = serviceProvider.GetRequiredService<InterviewsService>();
    }
    
    [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
    [HttpPatch("{id:guid}/confirm_offer")]
    public async Task<IActionResult> ConfirmOffer(Guid? id)
    {
        try
        {
            await _interviewsService.ConfirmOfferAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateInterview(NewInterviewRequest newInterviewRequest)
    {
        try
        {
            var result = await _interviewsService.CreateInterview(newInterviewRequest);
            return Ok(result);
        }
        catch (Exception)
        {
            return BadRequest("Unable to create interview.");
        }
    }

    [Authorize]
    [HttpGet("get/{id:guid}")]
    public async Task<IActionResult> GetInterview(Guid id)
    {
        try
        {
            var result = await _interviewsService.GetInterview(id);
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(id);
        }
        catch (Exception) 
        {
            return BadRequest("Unable to get interview.");
        }
    }

    /// <summary>
    /// Returns a list of student interviews
    /// </summary>
    [Authorize]
    [HttpGet("student/{studentId:guid}")]
    public async Task<IActionResult> GetStudentInterviews(Guid studentId)
    {
        try
        {
            var result = await _interviewsService.GetStudentInterviews(studentId);
            if (result == null)
            {
                throw new EntityNotFoundException();
            }
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(studentId);
        }
    }

    [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteInterview(Guid id)
    {
        try
        {
            await _interviewsService.DeleteInterview(id);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound(id);
        }
    }

    [Authorize]
    [HttpPatch("update_status/{id:guid}")]
    public async Task<IActionResult> UpdateInterview(Guid id, [FromBody] InterviewStateRequest newInterviewState)
    {
        try
        {
            var result = await _interviewsService.UpdateInterview(id, newInterviewState);
            return Ok(result);
        }
        catch (EntityNotFoundException)
        {
            return NotFound(id);
        }
    }

    [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllInterviews()
    {
        try
        {
            var result = await _interviewsService.GetAllInterviews();
            if (result == null)
            {
                throw new EntityNotFoundException();
            }
            return Ok(result);
        }
        catch (Exception)
        {
            return BadRequest("Unable to get all interviews");
        }
    }
}