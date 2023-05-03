using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
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
}