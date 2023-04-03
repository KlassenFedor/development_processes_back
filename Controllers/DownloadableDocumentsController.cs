using dev_processes_backend.Models.Dtos.DownloadableDocuments.RequestModels;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

// TODO allow for only admins and superadmins
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
}