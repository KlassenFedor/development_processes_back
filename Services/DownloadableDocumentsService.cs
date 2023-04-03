using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.DownloadableDocuments.RequestModels;

namespace dev_processes_backend.Services;

public class DownloadableDocumentsService : BaseService
{
    private readonly FilesService _filesService;

    public DownloadableDocumentsService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _filesService = serviceProvider.GetRequiredService<FilesService>();
    }
    
    public async Task UploadPracticeDiaryTemplateAsync(UploadPracticeDiaryTemplateRequestModel model)
    {
        var file = await _filesService.SaveFileAsync(model.File);

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
        var file = await _filesService.SaveFileAsync(model.File);

        var latestPracticeOrderThisYear = ApplicationDbContext.PracticeOrders
            .Where(pdt => pdt.StudyYearStart == GetCurrentStudyYear())
            .MaxBy(pdt => pdt.Version);
        var practiceOrder = new PracticeOrder
        {
            Id = new Guid(),
            StudyYearStart = GetCurrentStudyYear(),
            // каждый год минимальная версия - 1, все последующие документы имеют версию предыдущего + 1
            Version = latestPracticeOrderThisYear?.Version + 1 ?? 1,
            File = file
        };
        ApplicationDbContext.PracticeOrders.Add(practiceOrder);
        await ApplicationDbContext.SaveChangesAsync();
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