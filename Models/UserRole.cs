using Microsoft.AspNetCore.Identity;

namespace dev_processes_backend.Models;

public class UserRole : IdentityUserRole<Guid>, ISoftDeletableEntity
{
    public Guid Id { get; set; }
    
    public User User { get; set; }
    public Role Role { get; set; }

    public DateTime CreateDateTime { get; set; }
    public bool IsDeleted { get; set; } 
}