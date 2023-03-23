namespace dev_processes_backend.Models;

public class Vacancy : ISoftDeletableEntity
{
    public Guid Id { get; set; }
    public string Stack { get; set; }
    public string Description { get; set; }
    public string EstimatedNumberToHire { get; set; }
    public DateTime AppliableForDateStart { get; set; }
    public DateTime AppliableForDateEnd { get; set; }
    
    public Position Position { get; set; }
    
    public bool IsDeleted { get; set; }
}