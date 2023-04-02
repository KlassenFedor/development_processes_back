namespace dev_processes_backend.Models.Dtos.Vacancies.ResponseModels;

public class GetVacanciesElementResponseModel
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Stack { get; set; }
    public string Description { get; set; }
    public string EstimatedNumberToHire { get; set; }
    public DateTime AppliableForDateStart { get; set; }
    public DateTime AppliableForDateEnd { get; set; }
    public Position Position { get; set; }
}