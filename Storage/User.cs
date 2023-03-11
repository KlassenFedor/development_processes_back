using Microsoft.AspNetCore.Identity;

namespace dev_processes_backend.Storage
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; }
    }
}
