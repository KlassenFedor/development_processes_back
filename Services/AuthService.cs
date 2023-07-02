using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Security.Claims;

namespace dev_processes_backend.Services
{
    public class AuthService : BaseService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IServiceProvider serviceProvider,
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            ILogger<AuthService> logger) : base(serviceProvider)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task Login(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new KeyNotFoundException("Password incorrect or email does not exist");
            }

            var claims = new List<Claim>
            {
                new (ClaimTypes.Email, user.Email),
                new (ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            if (user.UserRoles?.Any() == true)
            {
                var roles = user.UserRoles.Select(x => x.Role).ToList();
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            }

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(2),
                IsPersistent = true
            };

            await _signInManager.SignInWithClaimsAsync(user, authProperties, claims);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IList<string>> GetUserRole(Guid? userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user != null)
                {
                    var userRole = await _userManager.GetRolesAsync(user);
                    return userRole;
                }
                throw new EntityNotFoundException();
            }
            catch (Exception ex)
            {
                _logger.LogError("Can not find user with userdId: " + userId.ToString() + " " + "error: " + ex.Message);
                throw new Exception();
            }
        }

        public async Task<bool> CheckIfTheSameUserOrAdmin(ClaimsPrincipal claimsPrincipal, Guid id)
        {
            Guid? userId = null;
            if (claimsPrincipal.Identity != null)
            {
                if (claimsPrincipal.Identity.IsAuthenticated)
                {
                    try
                    {
                        userId = Guid.Parse(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                        if (userId == null) 
                        {
                            throw new EntityNotFoundException();
                        }
                    }
                    catch
                    {
                        throw new EntityNotFoundException();
                    }
                }
                else
                {
                    throw new EntityNotFoundException();
                }
            }
            else
            {
                throw new EntityNotFoundException();
            }

            if (userId == id)
            {
                return true;
            }
            var userRole = await GetUserRole(userId);
            if (userRole.ToList()[0] == RolesNames.Administartor || userRole.ToList()[0] == RolesNames.SuperAdministrator)
            {
                return true;
            }

            return false;
        }

        private async Task Register(RegisterRequest model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Email = model.Email,
                Phone = model.Phone
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return;
            }
            throw new ArgumentException("We can not register this user");
        }

        public async Task<Guid> RegisterAdmin(RegisterAdminRequest model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Email = model.Email,
                Phone = model.Phone,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogError(result.Errors.First().Description);
                throw new ArgumentException("We can not register this user");
            }
            result = await _userManager.AddToRoleAsync(user, RolesNames.Administartor);
            if (!result.Succeeded)
            {
                _logger.LogError(result.Errors.First().Description);
                throw new ArgumentException("We can not add admin role to this user");
            }
            return user.Id;
        }

        public async Task<Guid> RegisterStudent(RegisterStudentRequest model)
        {
            var user = new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                Email = model.Email,
                Phone = model.Phone,
                Course = model.Course,
                Group = model.Group,
                EducationalTrack = model.EducationalTrack,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new ArgumentException("We can not register this user");
            }
            return user.Id;
        }

        public async Task ChangePassword(Guid? userId, string newPassword, string currentPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                throw new Exception();
            }
        }
    }
}
