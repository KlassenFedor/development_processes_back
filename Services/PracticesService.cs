using dev_processes_backend.Exceptions;
using dev_processes_backend.Models.Dtos.Practices.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace dev_processes_backend.Services;

public class PracticesService : BaseService
{
    private readonly FilesService _filesService;

    public PracticesService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _filesService = serviceProvider.GetRequiredService<FilesService>();
    }

    public async Task AddPracticeCharacterizationAsync(Guid? id, AddPracticeCharacterizationRequestModel model)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var practice = await ApplicationDbContext.Practices
            .Include(p => p.CharacterizationFile)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (practice == null)
        {
            throw new EntityNotFoundException();
        }
        
        var file = await _filesService.SaveFileAsync(model.File);
        practice.CharacterizationFile = file;
        ApplicationDbContext.Practices.Update(practice);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task GradePracticeAsync(Guid? id, int mark)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var practice = await ApplicationDbContext.Practices.FindAsync(id);
        if (practice == null)
        {
            throw new EntityNotFoundException();
        }

        if (!new[] {1, 2, 3, 4, 5}.Contains(mark))
        {
            throw new ArgumentException();
        }
        
        practice.CharacterizationMark = mark;
        ApplicationDbContext.Practices.Update(practice);
        await ApplicationDbContext.SaveChangesAsync();
    }


}