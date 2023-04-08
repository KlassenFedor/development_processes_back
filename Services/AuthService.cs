using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace dev_processes_backend.Services
{
    public interface IAuthService
    {
        Task Register(RegisterRequest model);
        Task Login(LoginRequest model);
        Task Logout();
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager,
                SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task Register(RegisterRequest model)
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
    }
}
