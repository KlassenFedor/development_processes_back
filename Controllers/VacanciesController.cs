﻿using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Vacancies.RequestModels;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dev_processes_backend.Controllers;

[Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
public class VacanciesController : BaseController
{
    private readonly VacanciesService _vacanciesService;
    private readonly VacanciesPrioritiesService _vacanciesPrioritiesService;

    public VacanciesController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _vacanciesService = serviceProvider.GetRequiredService<VacanciesService>();
        _vacanciesPrioritiesService = serviceProvider.GetRequiredService<VacanciesPrioritiesService>();
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

    [HttpPost("addVacancy/student/{studentId:guid}/vacancy/{vacancyId:guid}")]
    public async Task<IActionResult> AddVacancyToStudentList(Guid studentId, Guid vacancyId)
    {
        try
        {
            await _vacanciesPrioritiesService.AddVacancyToStudentsPrioritiesList(vacancyId, studentId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest("Unable to add vacancy to students list");
        }
    }
}