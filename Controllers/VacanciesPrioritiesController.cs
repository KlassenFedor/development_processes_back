using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.VacanciesPrioroties.RequestModels;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dev_processes_backend.Controllers
{
    public class VacanciesPrioritiesController : BaseController
    {
        private readonly VacanciesPrioritiesService _vacanciesPrioritiesService;

        public VacanciesPrioritiesController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _vacanciesPrioritiesService = serviceProvider.GetRequiredService<VacanciesPrioritiesService>();
        }

        [HttpPost("changePriorities/student/{studentId:guid}")]
        public async Task<IActionResult> ChangePriorities(Guid studentId, [FromBody] List<ChangeVacanciyPriorityRequest> newVacanciesPriorities)
        {
            try
            {
                await _vacanciesPrioritiesService.ChangePriorities(studentId, newVacanciesPriorities);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Unable to change priorities.");
            }
        }
    }
}
