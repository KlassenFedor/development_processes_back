using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Companies.RequestModels;
using dev_processes_backend.Models.Dtos.Companies.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace dev_processes_backend.Services;

public class CompaniesService : BaseService
{
    private readonly FilesService _filesService;

    public CompaniesService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _filesService = serviceProvider.GetRequiredService<FilesService>();
    }
    
    public Task<List<GetCompaniesElementResponseModel>> GetCompaniesAsync()
    {
        return ApplicationDbContext.Companies
            .Include(c => c.Logo)
            .Select(c => new GetCompaniesElementResponseModel
            {
                Id = c.Id,
                Name = c.Name,
                Site = c.Site,
                Information = c.Information,
                LogoUrl = c.Logo.Path
            }).ToListAsync();
    }

    public async Task<CreateCompanyResponseModel> CreateCompanyAsync(CreateCompanyRequestModel model)
    {
        var company = new Company
        {
            Id = new Guid(),
            Name = model.Name,
            Site = model.Site,
            Information = model.Information,
        };
        ApplicationDbContext.Companies.Add(company);
        await ApplicationDbContext.SaveChangesAsync();
        return new CreateCompanyResponseModel
        {
            Id = company.Id
        };
    }

    public async Task EditCompanyAsync(Guid? id, EditCompanyRequestModel model)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var company = await ApplicationDbContext.Companies.FindAsync(id);
        if (company == null)
        {
            throw new EntityNotFoundException();
        }
        company.Name = model.Name ?? company.Name;
        company.Site = model.Site ?? company.Site;
        company.Information = model.Information ?? company.Information;
        ApplicationDbContext.Companies.Update(company);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task DeleteCompanyAsync(Guid? id)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var company = await ApplicationDbContext.Companies.FindAsync(id);
        if (company == null)
        {
            throw new EntityNotFoundException();
        }
        ApplicationDbContext.Companies.Remove(company);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task UploadLogoAsync(Guid? companyId, UploadLogoRequestModel model)
    {
        if (companyId == null)
        {
            throw new EntityNotFoundException();
        }
        var company = await ApplicationDbContext.Companies
            .Include(c => c.Logo)
            .FirstOrDefaultAsync(c => c.Id == companyId);
        if (company == null)
        {
            throw new EntityNotFoundException();
        }

        var file = await _filesService.SaveFileAsync(model.File);
        company.Logo = file;
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task<GetCompaniesElementResponseModel> GetCompanyAsync(Guid? companyId)
    {
        if (companyId == null)
        {
            throw new EntityNotFoundException();
        }
        var company = await ApplicationDbContext.Companies.Include(c => c.Logo).FirstOrDefaultAsync(c => c.Id == companyId);
        if (company == null)
        {
            throw new EntityNotFoundException();
        }
        return new GetCompaniesElementResponseModel
        {
            Id = company.Id,
            Name = company.Name,
            Site = company.Site,
            Information = company.Information,
            LogoUrl = company.Logo == null ? null : company.Logo.Path
        };
    }
}