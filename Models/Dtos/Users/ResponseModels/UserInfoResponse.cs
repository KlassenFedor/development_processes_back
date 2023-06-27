namespace dev_processes_backend.Models.Dtos.Users.ResponseModels
{
    public class UserInfoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
    }
}
