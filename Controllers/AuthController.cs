using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.Auth;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
                await _authService.RegisterAdmin(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Incorrect data");
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
                await _authService.RegisterStudent(model);
                return Ok();
            }
            catch
            {
                return BadRequest("Incorrect data");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
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
