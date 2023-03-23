using Microsoft.AspNetCore.Identity;

namespace dev_processes_backend.Models;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public override string Email { get; set; }
    public string? Phone { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; }
}