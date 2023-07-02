using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.DownloadableDocuments.RequestModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dev_processes_backend.Services;

public class DownloadableDocumentsService : BaseService
{
    private readonly FilesService _filesService;
    private readonly string _uploadsLocation;

    public DownloadableDocumentsService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
    {
        _filesService = serviceProvider.GetRequiredService<FilesService>();
        _uploadsLocation = configuration["UploadsLocation"] ??
                           throw new ArgumentNullException(message: "Не указана директория для загрузки файлов", null);
    }
    
    public async Task UploadPracticeDiaryTemplateAsync(UploadPracticeDiaryTemplateRequestModel model)
    {
        var file = await _filesService.SaveFileAsync(model.File, "practiceDiaryTemplate.pdf");

        var documentsThisYear = ApplicationDbContext.PracticeDiaryTemplates
            .Where(pdt => pdt.StudyYearStart == GetCurrentStudyYear())
            .ToList();
        var latestDocumentThisYear = documentsThisYear.MaxBy(pdt => pdt.Version);
        var practiceDiaryTemplate = new PracticeDiaryTemplate
        {
            Id = new Guid(),
            StudyYearStart = GetCurrentStudyYear(),
            // каждый год минимальная версия - 1, все последующие документы имеют версию предыдущего + 1
            Version = latestDocumentThisYear?.Version + 1 ?? 1,
            File = file
        };
        ApplicationDbContext.PracticeDiaryTemplates.Add(practiceDiaryTemplate);
        await ApplicationDbContext.SaveChangesAsync();
    }
    
    public async Task UploadPracticeOrderAsync(UploadPracticeOrderRequestModel model)
    {
        var file = await _filesService.SaveFileAsync(model.File, "practiceOrder.pdf");

        var latestPracticeOrderThisYear = await ApplicationDbContext.PracticeOrders
            .Where(pdt => pdt.StudyYearStart.Equals(GetCurrentStudyYear())).ToListAsync();
        PracticeOrder? version = null;
        if (latestPracticeOrderThisYear != null)
        {
            version = latestPracticeOrderThisYear.MaxBy(pdt => pdt.Version);
        }
        var practiceOrder = new PracticeOrder
        {
            Id = new Guid(),
            StudyYearStart = GetCurrentStudyYear(),
            // каждый год минимальная версия - 1, все последующие документы имеют версию предыдущего + 1
            Version = version?.Version + 1 ?? 1,
            File = file
        };
        ApplicationDbContext.PracticeOrders.Add(practiceOrder);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public string DownloadPracticeDiaryTemplate()
    {
        var directory = Path.Combine(Directory.GetCurrentDirectory(), _uploadsLocation, "practiceDiaryTemplate.pdf");
        return directory;
    }

    public string DownloadPracticeOrder()
    {
        var directory = Path.Combine(Directory.GetCurrentDirectory(), _uploadsLocation, "practiceOrder.pdf");
        return directory;
    }

    private static int GetCurrentStudyYear()
    {
        var currentStudyYear = DateTime.Now.Year;
        if (DateTime.Compare(new DateTime(currentStudyYear, 9, 1), DateTime.Now) > 0)
        {
            currentStudyYear--;
        }

        return currentStudyYear;
    }
}