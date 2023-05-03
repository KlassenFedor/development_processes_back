using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Vacancies.RequestModels;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

[Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
public class VacanciesController : BaseController
{
    private readonly VacanciesService _vacanciesService;
    
    public VacanciesController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _vacanciesService = serviceProvider.GetRequiredService<VacanciesService>();
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok(await _vacanciesService.GetVacanciesAsync());
    }

    /// <summary>
    /// Требуется дата в формате 2012-04-23T18:25:43.511Z. Позиция задается цифрой в енаме
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVacancyRequestModel model)
    {   
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            var response = await _vacanciesService.CreateVacancyAsync(model);
            return CreatedAtRoute(null, response);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Требуется дата в формате 2012-04-23T18:25:43.511Z. Позиция задается цифрой в енаме
    /// </summary>
    /// <returns></returns>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Edit(Guid? id, [FromBody] EditVacancyRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        try
        {
            await _vacanciesService.EditVacancyAsync(id, model);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid? id)
    {
        try
        {
            await _vacanciesService.DeleteVacancyAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
}