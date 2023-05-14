namespace dev_processes_backend.Models.Dtos.Users.ResponseModel
{
    public class StudentResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public int Course { get; set; }
        public string Group { get; set; }
        public EducationalTrack? EducationalTrack { get; set; }
    }
}
