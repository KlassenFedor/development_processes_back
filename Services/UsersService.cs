using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Users.ResponseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace dev_processes_backend.Services;

public class UsersService : BaseService
{
    private readonly UserManager<User> _userManager;
    public UsersService(IServiceProvider serviceProvider, UserManager<User> userManager) : base(serviceProvider)
    {
        _userManager = userManager;
    }

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
            await ApplicationDbContext.SaveChangesAsync();
        }
        else
        {
            throw new EntityNotFoundException();
        }
    }
    
    public async Task<List<GetAdminsElementResponseModel>> GetAdminsAsync()
    {
        var admins = await ApplicationDbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Where(u => u.UserRoles.Any(ur => ur.Role.Type == RoleType.Admin))
            .ToListAsync();
        
        return admins.Select(u => new GetAdminsElementResponseModel
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Patronymic = u.Patronymic,
            Email = u.Email,
            CreateDateTime = u.UserRoles.Single(ur => ur.Role.Type == RoleType.Admin).CreateDateTime,
            IsActive = !u.UserRoles.Single(ur => ur.Role.Type == RoleType.Admin).IsDeleted
        }).ToList();
    }

    public async Task DeleteUserAsync(Guid? userId, Guid requesterId)
    {
        if (userId == null)
        {
            throw new EntityNotFoundException();
        }
        var user = await ApplicationDbContext.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        // must exist according to the Authorize attribute on the controller,
        // so the requester is either an admin or a superadmin
        var requesterRoles = ApplicationDbContext.Roles
            .Include(r => r.UserRoles)
            .ThenInclude(ur => ur.User)
            .Where(r => r.UserRoles.Any(ur => ur.UserId == requesterId));
        foreach (var requesterRole in requesterRoles)
        {
            Console.WriteLine($"ROLES HERE {requesterRole.Type}");    
        }
        
        // the requester is an admin and the user is either an admin or a superadmin
        if (requesterRoles.All(rr => rr.Type == RoleType.Admin) && user.UserRoles.Any())
        {
            Console.WriteLine("EXC HERE");
            throw new NoAccessException();
        }

        ApplicationDbContext.Users.Remove(user);
        await ApplicationDbContext.SaveChangesAsync();
    }

    public async Task<UserInfoResponse> GetUserInformation(Guid? userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        return new UserInfoResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<UserInfoResponse> GetAdminInformation(Guid? userId)
    {
        var users = await _userManager.GetUsersInRoleAsync(RolesNames.Administartor);
        User administrator = null;
        foreach (var user in users)
        {
            if (user.Id == userId)
            {
                administrator = user;
            }
        }
        if (administrator == null)
        {
            throw new EntityNotFoundException();
        }
        return new UserInfoResponse
        {
            FirstName = administrator.FirstName,
            LastName = administrator.LastName,
            Email = administrator.Email
        };
    }

    public async Task<UserInfoResponse> GetStudentInformation(Guid? userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        if (
            await _userManager.IsInRoleAsync(user, RolesNames.Administartor) ||
            await _userManager.IsInRoleAsync(user, RolesNames.SuperAdministrator)
            )
        {
            throw new EntityNotFoundException();
        }
        return new UserInfoResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}