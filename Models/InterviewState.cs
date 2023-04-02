namespace dev_processes_backend.Models;

public class InterviewState
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public string? Description { get; set; }
    
    public InterviewStatus Status { get; set; }
}