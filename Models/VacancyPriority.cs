namespace dev_processes_backend.Models;

public class VacancyPriority
{
    public Guid Id { get; set; }
    public int Value { get; set; }
    
    public Vacancy Vacancy { get; set; }
}