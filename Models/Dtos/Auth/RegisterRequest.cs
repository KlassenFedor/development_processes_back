namespace dev_processes_backend.Models.Dtos.Auth
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public  string Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
    }
}
