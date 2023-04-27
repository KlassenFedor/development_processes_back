using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Auth;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;

namespace dev_processes_backend.Controllers
{
    public class AuthController : BaseController
    {
        private readonly AuthService _authService;
        private readonly ILogger _logger;

        public AuthController(IServiceProvider serviceProvider, ILogger<AuthController> logger) : base(serviceProvider)
        {
            _authService = serviceProvider.GetRequiredService<AuthService>();
            _logger = logger;
        }

        [Authorize(Roles = RolesNames.SuperAdministrator)]
        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminRequest model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, "Invalid input data.");
            }
            try
            {
                var result = await _authService.RegisterAdmin(model);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Incorrect data");
            }
        }

        [HttpGet("get/role")]
        public async Task<IActionResult> GetRole()
        {
            try
            {
                Guid? userId = null;
                if (User.Identity != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        try
                        {
                            userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                            var result = await _authService.GetUserRole(userId);
                            return Ok(result);
                        }
                        catch
                        {
                            return BadRequest("Can not find user role");
                        }
                    }
                }
                return Unauthorized("User not authorized");
            }
            catch(Exception e) 
            {
                return BadRequest("Unexpected error");
            }
        }

        [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
        [HttpPost("register/student")]
        public async Task<IActionResult> RegisterStudent(RegisterStudentRequest model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, "Invalid input data.");
            }
            try
            {
                var result = await _authService.RegisterStudent(model);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Incorrect data");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, "Invalid input data.");
            }
            try
            {
                await _authService.Login(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest("Incorrect email or password");
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task Logout()
        {
            await _authService.Logout();
        }
    }
}
