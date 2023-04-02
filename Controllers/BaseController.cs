using Microsoft.AspNetCore.Mvc;

namespace dev_processes_backend.Controllers;

[ApiController]
[Route("[controller]")]
public class BaseController : ControllerBase
{
    protected BaseController(IServiceProvider serviceProvider) {}
}