using Microsoft.AspNetCore.Identity;

namespace dev_processes_backend.Models;

public class Role : IdentityRole<Guid>
{
    public override Guid Id { get; set; }
    public RoleType Type { get; set; }
    
    public ICollection<UserRole> UserRoles { get; set; }
}