using System.ComponentModel.DataAnnotations.Schema;

namespace dev_processes_backend.Models.Dtos.Interviews.InterviewResponse
{
    public class InterviewResponse
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public Vacancy Vacancy { get; set; }
        public ICollection<InterviewState> History { get; set; }
        public Student Student { get; set; }
        public InterviewState CurrentState => History.Last();
    }
}
