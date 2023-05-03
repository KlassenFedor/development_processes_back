using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Companies.RequestModels;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

[Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
public class CompaniesController : BaseController
{
    private readonly CompaniesService _companiesService;
    
    public CompaniesController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _companiesService = serviceProvider.GetRequiredService<CompaniesService>();
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return Ok(await _companiesService.GetCompaniesAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequestModel model)
    {   
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var response = await _companiesService.CreateCompanyAsync(model);
        return CreatedAtRoute(null, response);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Edit(Guid? id, [FromBody] EditCompanyRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        try
        {
            await _companiesService.EditCompanyAsync(id, model);
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
            await _companiesService.DeleteCompanyAsync(id);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Передаваемый в форме файл должен иметь ключ "file" 
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{companyId:guid}/logo")]
    public async Task<IActionResult> UploadLogo(Guid? companyId, [FromForm] UploadLogoRequestModel model)
    {
        try
        {
            await _companiesService.UploadLogoAsync(companyId, model);
            return Ok();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
}