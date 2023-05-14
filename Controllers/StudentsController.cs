using dev_processes_backend.Models.Dtos.Auth;
using dev_processes_backend.Models;
using dev_processes_backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using dev_processes_backend.Models.Dtos.Users.RequestModels;

namespace dev_processes_backend.Controllers
{
    public class StudentsController: BaseController
    {
        private readonly StudentsService _studentsService;

        public StudentsController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _studentsService = serviceProvider.GetRequiredService<StudentsService>();
        }

        [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
        [HttpGet("get/{studentId:guid}")]
        public async Task<IActionResult> GetStudent(Guid studentId)
        {
            try {
                var result = await _studentsService.GetStudent(studentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Can not find student with the specified id.");
            }
        }

        [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
        [HttpGet("get/group/{group}")]
        public async Task<IActionResult> GetStudent(string group)
        {
            try
            {
                var result = await _studentsService.GetGroupStudents(group);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Can not find students for specified group.");
            }
        }

        [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
        [HttpGet("get/course/{course}")]
        public async Task<IActionResult> GetCourseStudents(int course)
        {
            try
            {
                var result = await _studentsService.GetCourseStudents(course);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Can not find students for specified course.");
            }
        }

        [Authorize(Roles = RolesNames.SuperAdministrator + "," + RolesNames.Administartor)]
        [HttpPost("edit/{studentId:guid}")]
        public async Task<IActionResult> GetStudent(Guid studentId, EditStudentRequest model)
        {
            try
            {
                await _studentsService.EditStudent(studentId, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error during student update.");
            }
        }
    }
}
