using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace dev_processes_backend.Services;

public class UsersService : BaseService
{
    public UsersService(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public async Task BlockAdminAsync(Guid? id)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var user = await ApplicationDbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        var userRole = user.UserRoles.FirstOrDefault(ur => ur.Role.Type == RoleType.Admin && !ur.IsDeleted);
        if (userRole != null)
        {
            ApplicationDbContext.UserRoles.Remove(userRole);
            ApplicationDbContext.UserRoles.Update(userRole);
            await ApplicationDbContext.SaveChangesAsync();
        }
        else
        {
            throw new EntityNotFoundException();
        }
    }
    
    public async Task UnblockAdminAsync(Guid? id)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var user = await ApplicationDbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new EntityNotFoundException();
        }

        var userRole = user.UserRoles.FirstOrDefault(ur => ur.Role.Type == RoleType.Admin && ur.IsDeleted);
        if (userRole != null)
        {
            userRole.IsDeleted = false;
            ApplicationDbContext.UserRoles.Update(userRole);
            // TODO возможно также потребуется вручную обновить состояние юзерроли в контексте, если возникнет странное поведение
            await ApplicationDbContext.SaveChangesAsync();
        }
        else
        {
            throw new EntityNotFoundException();
        }
    }
}