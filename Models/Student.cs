namespace dev_processes_backend.Models;

public class Student : User
{
    public int Course { get; set; }
    public string Group { get; set; }
    public EducationalTrack EducationalTrack { get; set; }
    
    public ICollection<VacancyPriority> VacancyPriorities { get; set; }
    public ICollection<Practice> Practices { get; set; }
    public ICollection<Interview> Interviews { get; set; }
}