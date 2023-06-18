namespace dev_processes_backend.Models.Dtos.Interviews
{
    public class InterviewStateRequest
    {
        public DateTime DateTime { get; set; }
        public InterviewStatus Status { get; set; }
    }
}
