namespace dev_processes_backend.Models;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Site { get; set; }
    public string Information { get; set; }
    
    public Guid? LogoId { get; set; }
    public File? Logo { get; set; }
    public ICollection<Vacancy> Vacancies { get; set; }
}