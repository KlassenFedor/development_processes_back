using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Companies.RequestModels;
using dev_processes_backend.Models.Dtos.Companies.ResponseModels;
using dev_processes_backend.Models.Dtos.Vacancies.RequestModels;
using dev_processes_backend.Models.Dtos.Vacancies.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace dev_processes_backend.Services;

public class VacanciesService : BaseService
{
    public VacanciesService(IServiceProvider serviceProvider) : base(serviceProvider) { }
    
    public Task<List<GetVacanciesElementResponseModel>> GetVacanciesAsync()
    {
        return ApplicationDbContext.Vacancies
            .Include(v => v.Company)
            .Select(v => new GetVacanciesElementResponseModel
            {
                Id = v.Id,
                CompanyId = v.Company.Id,
                CompanyName = v.Company.Name,
                Stack = v.Stack,
                Description = v.Description,
                EstimatedNumberToHire = v.EstimatedNumberToHire,
                AppliableForDateStart = v.AppliableForDateStart,
                AppliableForDateEnd = v.AppliableForDateEnd,
                Position = v.Position
            }).ToListAsync();
    }

    public async Task<CreateVacancyResponseModel> CreateVacancyAsync(CreateVacancyRequestModel model)
    {
        var company = await ApplicationDbContext.Companies.FindAsync(model.CompanyId);
        if (company == null)
        {
            throw new EntityNotFoundException();
        }

        var vacancy = new Vacancy
        {
            Id = new Guid(),
            Stack = model.Stack,
            Description = model.Description,
            EstimatedNumberToHire = model.EstimatedNumberToHire,
            AppliableForDateStart = model.AppliableForDateStart,
            AppliableForDateEnd = model.AppliableForDateEnd,
            Company = company,
            Position = model.Position
        };

        ApplicationDbContext.Vacancies.Add(vacancy);
        await ApplicationDbContext.SaveChangesAsync();
        return new CreateVacancyResponseModel
        {
            Id = vacancy.Id
        };
    }

    public async Task EditVacancyAsync(Guid? id, EditVacancyRequestModel model)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var vacancy = await ApplicationDbContext.Vacancies
            .Include(v => v.Company)
            .FirstOrDefaultAsync(v => v.Id == id);
        var company = await ApplicationDbContext.Companies.FindAsync(model.CompanyId);
        if (vacancy == null || model.CompanyId != null && company == null)
        {
            throw new EntityNotFoundException();
        }
        
        vacancy.Stack = model.Stack ?? vacancy.Stack;
        vacancy.Description = model.Description ?? vacancy.Description;
        vacancy.EstimatedNumberToHire = model.EstimatedNumberToHire ?? vacancy.EstimatedNumberToHire;
        vacancy.AppliableForDateStart = model.AppliableForDateStart ?? vacancy.AppliableForDateStart;
        vacancy.AppliableForDateEnd = model.AppliableForDateEnd ?? vacancy.AppliableForDateEnd;
        vacancy.Company = company ?? vacancy.Company;
        vacancy.Position = model.Position ?? vacancy.Position;
        ApplicationDbContext.Vacancies.Update(vacancy);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteVacancyAsync(Guid? id)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var vacancy = await ApplicationDbContext.Vacancies.FindAsync(id);
        if (vacancy == null)
        {
            throw new EntityNotFoundException();
        }
        ApplicationDbContext.Vacancies.Remove(vacancy);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task<List<GetVacanciesElementResponseModel>> GetCompanyVacancies(Guid companyId)
    {
        var result = await ApplicationDbContext.Vacancies
            .Include(v => v.Company)
            .Where(v => v.Company.Id == companyId)
            .Select(v => new GetVacanciesElementResponseModel 
            {
                Id = v.Id,
                CompanyId = v.Company.Id,
                CompanyName = v.Company.Name,
                Stack = v.Stack,
                Description = v.Description,
                EstimatedNumberToHire = v.EstimatedNumberToHire,
                AppliableForDateStart = v.AppliableForDateStart,
                AppliableForDateEnd = v.AppliableForDateEnd,
                Position = v.Position
            }).ToListAsync();

        return result;
    }

    public async Task<List<GetVacanciesElementResponseModel>> GetStudentVacancies(Guid? studentId)
    {
        if (studentId == null)
        {
            throw new EntityNotFoundException();
        }
        var student = await ApplicationDbContext.Students.Include(s => s.VacancyPriorities).ThenInclude(vp => vp.Vacancy).ThenInclude(v => v.Company).FirstOrDefaultAsync(s => s.Id == studentId);
        var vacanciesIds = student.VacancyPriorities.ToList().Select(vp => vp.Vacancy).Select(v => v.Id);
        var vacancies = ApplicationDbContext.Vacancies.Where(v => vacanciesIds.Contains(v.Id)).ToList()
            .Select(v => new GetVacanciesElementResponseModel
            {
                Id = v.Id,
                CompanyId = v.Company.Id,
                CompanyName = v.Company.Name,
                Stack = v.Stack,
                Description = v.Description,
                EstimatedNumberToHire = v.EstimatedNumberToHire,
                AppliableForDateStart = v.AppliableForDateStart,
                AppliableForDateEnd = v.AppliableForDateEnd,
                Position = v.Position
            }).ToList();

        return vacancies;
    }

    public async Task DeleteStudentVacancy(Guid? studentId, Guid? vacancyId)
    {
        if (studentId == null || vacancyId == null)
        {
            throw new EntityNotFoundException();
        }
        var student = await ApplicationDbContext.Students.Include(s => s.VacancyPriorities).ThenInclude(vp => vp.Vacancy).FirstOrDefaultAsync(s => s.Id == studentId);
        if (student == null)
        {
            throw new EntityNotFoundException();
        }
        var vacancyPriority = student.VacancyPriorities.Where(vp => vp.Vacancy.Id == vacancyId).FirstOrDefault();
        if (vacancyPriority == null)
        {
            throw new EntityNotFoundException();
        }
        student.VacancyPriorities.Remove(vacancyPriority);
        await ApplicationDbContext.SaveChangesAsync();
    }
}