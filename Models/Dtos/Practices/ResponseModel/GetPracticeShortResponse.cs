namespace dev_processes_backend.Models.Dtos.Practices.ResponseModel
{
    public class GetPracticeShortResponse
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Course { get; set; }
        public int? CharacterizationMark { get; set; }
        public Guid CompanyId { get; set; }
        public int Position { get; set; }
    }
}
