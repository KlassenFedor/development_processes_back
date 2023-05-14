using System.ComponentModel.DataAnnotations.Schema;

namespace dev_processes_backend.Models.Dtos.Interviews.InterviewRequest
{
    public class NewInterviewRequest
    {
        public string? Description { get; set; }
        public Guid VacancyId { get; set; }
        public Guid? StudentId { get; set; }
        public InterviewState? InterviewState { get; set; }
    }
}
