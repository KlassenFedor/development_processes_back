namespace dev_processes_backend.Models.Dtos.Practices.RequestModels
{
    public class AddPracticeRequest
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Course { get; set; }
        public Guid CompanyId { get; set; }
        public int Position { get; set; }
    }
}
