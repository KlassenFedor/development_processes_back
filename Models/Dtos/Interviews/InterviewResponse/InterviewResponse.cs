using System.ComponentModel.DataAnnotations.Schema;

namespace dev_processes_backend.Models.Dtos.Interviews.InterviewResponse
{
    public class InterviewResponse
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public Guid VacancyId { get; set; }
        public ICollection<InterviewState> History { get; set; }
        public Guid StudentId { get; set; }
        public InterviewStateResponse CurrentState => new InterviewStateResponse { 
            DateTime = History.Last().DateTime,
            Status = History.Last().Status
        };
    }
}
