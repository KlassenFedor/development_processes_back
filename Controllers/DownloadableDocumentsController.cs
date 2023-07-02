using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.DownloadableDocuments.RequestModels;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

[Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
public class DownloadableDocumentsController : BaseController
{
    private readonly DownloadableDocumentsService _downloadableDocumentsService;

    public DownloadableDocumentsController(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _downloadableDocumentsService = serviceProvider.GetRequiredService<DownloadableDocumentsService>();
    }
    
    /// <summary>
    /// Передаваемый в форме файл должен иметь ключ "file" 
    /// </summary>
    /// <returns></returns>
    [HttpPost("practice_diary_template")]
    public async Task<IActionResult> PracticeDiaryTemplate([FromForm] UploadPracticeDiaryTemplateRequestModel model)
    {
        await _downloadableDocumentsService.UploadPracticeDiaryTemplateAsync(model);
        return Ok();
    }

    /// <summary>
    /// Передаваемый в форме файл должен иметь ключ "file" 
    /// </summary>
    /// <returns></returns>
    [HttpPost("practice_order")]
    public async Task<IActionResult> PracticeOrder([FromForm] UploadPracticeOrderRequestModel model)
    {
        await _downloadableDocumentsService.UploadPracticeOrderAsync(model);
        return Ok();
    }

    [HttpGet("practice_diary_template")]
    public IActionResult GetPracticeDiaryTemplate()
    {
        return Ok(_downloadableDocumentsService.DownloadPracticeDiaryTemplate());
    }

    [HttpGet("practice_order")]
    public IActionResult GetPracticeOrderte()
    {
        return Ok(_downloadableDocumentsService.DownloadPracticeOrder());
    }
}