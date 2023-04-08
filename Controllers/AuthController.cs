using dev_processes_backend.Models.Dtos.Auth;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace dev_processes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private readonly ILogger _logger;

        public AuthController(IAuthService service, ILogger<AuthController> logger)
        {
            _authService = service;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, "Invalid input data.");
            }
            try
            {
                await _authService.Register(model);
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
