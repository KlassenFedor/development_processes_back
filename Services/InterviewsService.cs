using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Interviews;
using dev_processes_backend.Models.Dtos.Interviews.InterviewRequest;
using dev_processes_backend.Models.Dtos.Interviews.InterviewResponse;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.EntityFrameworkCore;

namespace dev_processes_backend.Services;

public class InterviewsService : BaseService
{
    public InterviewsService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async Task ConfirmOfferAsync(Guid? id)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var interview = await ApplicationDbContext.Interviews
            .Include(i => i.History)
            .FirstOrDefaultAsync(i => i.Id == id);
        if (interview == null)
        {
            throw new EntityNotFoundException();
        }
        
        interview.History.Add(new InterviewState
        {
            Id = new Guid(),
            DateTime = DateTime.Now,
            Status = InterviewStatus.OfferConfirmed
        });
        
        ApplicationDbContext.Interviews.Update(interview);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task<Guid> CreateInterview(NewInterviewRequest newInterviewRequest)
    {
        var newInterview = new Interview();
        newInterview.History = new List<InterviewState>();
        newInterview.Description = newInterviewRequest.Description;

        var student  = await ApplicationDbContext.Students.FirstOrDefaultAsync(s => s.Id == newInterviewRequest.StudentId);
        if (student == null)
        {
            throw new EntityNotFoundException();
        }
        newInterview.Student = student;

        var vacancy = await ApplicationDbContext.Vacancies.FirstOrDefaultAsync(v => v.Id == newInterviewRequest.VacancyId);
        if (vacancy == null)
        {
            throw new EntityNotFoundException();
        }

        newInterview.Vacancy = vacancy;

        var interviewState = new InterviewState
        {
            DateTime = newInterviewRequest.InterviewState.DateTime,
            Status = newInterviewRequest.InterviewState.Status
        };

        await ApplicationDbContext.Interviews.AddAsync(newInterview);

        if (newInterviewRequest.InterviewState != null)
        {
            newInterview.History.Add(interviewState);
        }

        await ApplicationDbContext.SaveChangesAsync();

        return newInterview.Id;
    }

    public async Task<InterviewResponse> GetInterview(Guid interviewId)
    {
        var interview = await ApplicationDbContext.Interviews
            .Include(i => i.Vacancy)
            .Include(i => i.Student)
            .Select(i => new InterviewResponse 
            {
                Id = i.Id,
                Description = i.Description,
                VacancyId = i.Vacancy.Id,
                History = i.History,
                StudentId = i.Student.Id
            })
            .FirstOrDefaultAsync(i => i.Id == interviewId);
        if (interview == null)
        {
            throw new EntityNotFoundException();
        }

        return interview;
    }

    public async Task<List<InterviewResponse>> GetStudentInterviews(Guid studentId)
    {
        var interviews = await ApplicationDbContext.Interviews
            .Include(i => i.Vacancy)
            .Include(i => i.Student)
            .Where(i => i.Student.Id == studentId)
            .Select(i => new InterviewResponse
            {
                Id = i.Id,
                Description = i.Description,
                VacancyId = i.Vacancy.Id,
                History = i.History,
                StudentId = i.Student.Id
            })
            .ToListAsync();

        return interviews;
    }

    public async Task DeleteInterview(Guid interviewId)
    {
        var interview = await ApplicationDbContext.Interviews
            .Include(i => i.History)
            .Where(i => i.Id == interviewId).FirstOrDefaultAsync();
        if (interview == null)
        {
            throw new EntityNotFoundException();
        }
        ApplicationDbContext.Interviews.Remove(interview);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task<Guid> UpdateInterview(Guid interviewId, InterviewStateRequest newInterviewState)
    {
        var interview = await ApplicationDbContext.Interviews.Include(i => i.History).Where(i => i.Id == interviewId).FirstOrDefaultAsync();
        var interviewState = new InterviewState
        {
            DateTime = newInterviewState.DateTime,
            Status = newInterviewState.Status
        };
        if (interview == null)
        {
            throw new EntityNotFoundException();
        }
        interview.History.Add(interviewState);
        await ApplicationDbContext.SaveChangesAsync();

        return interviewId;
    }
}