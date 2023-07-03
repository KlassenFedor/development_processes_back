using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Practices.RequestModels;
using dev_processes_backend.Models.Dtos.Practices.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

    public async Task<Guid> AddPractice(Guid? userId, AddPracticeRequest practiceRequest)
    {
        if (userId == null)
        {
            Debug.WriteLine("user not found");
            throw new EntityNotFoundException();
        }
        var student = await ApplicationDbContext.Students.Include(s => s.Practices).FirstOrDefaultAsync(u => u.Id == userId);
        if (student == null)
        {
            Debug.WriteLine("student not found");
            throw new EntityNotFoundException();
        }
        var company = await ApplicationDbContext.Companies.FirstOrDefaultAsync(c => c.Id == practiceRequest.CompanyId);
        if (company == null)
        {
            throw new EntityNotFoundException();
        }
        Practice practice = new Practice
        {
            DateStart = practiceRequest.DateStart,
            DateEnd = practiceRequest.DateEnd,
            Course = practiceRequest.Course,
            CharacterizationMark = 1,
            Position = (Position)practiceRequest.Position,
            Company = company
        };
        student.Practices.Add(practice);
        await ApplicationDbContext.SaveChangesAsync();

        return practice.Id;
    }

    public async Task<GetPracticeResponse> GetPractice(Guid? practiceId)
    {
        if (practiceId == null)
        {
            throw new EntityNotFoundException();
        }
        var practice = await ApplicationDbContext.Practices
            .Include(p => p.PracticeDiary)
            .Include(p => p.CharacterizationFile)
            .Include(p => p.Company)
            .FirstOrDefaultAsync(p => p.Id == practiceId);
        if (practice == null)
        {
            throw new EntityNotFoundException();
        }

        return new GetPracticeResponse
        {
            DateStart = practice.DateStart,
            DateEnd = practice.DateEnd,
            Course = practice.Course,
            CharacterizationMark = practice.CharacterizationMark,
            Position = (int)practice.Position,
            CompanyId = practice.Company.Id,
            PracticeDiary = practice.PracticeDiary,
            CharacterizationFile = practice.CharacterizationFile
        };
    }

    public async Task AddPracticeDiary(Guid? practiceId, AddPracticeDiaryRequest diary)
    {
        if (practiceId == null)
        {
            throw new EntityNotFoundException();
        }
        var practice = await ApplicationDbContext.Practices
            .Include(p => p.PracticeDiary)
            .FirstOrDefaultAsync(p => p.Id == practiceId);
        if (practice == null)
        {
            throw new EntityNotFoundException();
        }

        var file = await _filesService.SaveFileAsync(diary.File);
        practice.PracticeDiary = file;
        ApplicationDbContext.Practices.Update(practice);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task<List<GetPracticeShortResponse>> GetStudentPractices(Guid? userId)
    {
        if (userId == null)
        {
            throw new EntityNotFoundException();
        }
        var student = await ApplicationDbContext.Students.Include(s => s.Practices).ThenInclude(p => p.Company).FirstOrDefaultAsync(s => s.Id == userId);
        if (student == null)
        {
            throw new EntityNotFoundException();
        }
        var practices = student.Practices.ToList();

        return practices.Select(p => new GetPracticeShortResponse
        {
            DateStart = p.DateStart,
            DateEnd = p.DateEnd,
            Course = p.Course,
            CharacterizationMark = p.CharacterizationMark,
            Position = (int)p.Position,
            CompanyId = p.Company.Id,
        }).ToList();
    }
}