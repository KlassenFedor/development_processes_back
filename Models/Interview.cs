using System.ComponentModel.DataAnnotations.Schema;

namespace dev_processes_backend.Models;

public class Interview
{
    public Guid Id { get; set; }
    public string? Description { get; set; }

    public Vacancy Vacancy { get; set; }
    public ICollection<InterviewState> History { get; set; }
    public Student Student { get; set; }
    
    [NotMapped]
    public InterviewState CurrentState => History.Last();
}