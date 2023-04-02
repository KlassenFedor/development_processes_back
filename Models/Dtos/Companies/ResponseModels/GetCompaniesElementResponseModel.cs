namespace dev_processes_backend.Models.Dtos.Companies.ResponseModels;

public class GetCompaniesElementResponseModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Site { get; set; }
    public string Information { get; set; }
    public string LogoUrl { get; set; }
}