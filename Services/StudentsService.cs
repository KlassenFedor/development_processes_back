using dev_processes_backend.Exceptions;
using dev_processes_backend.Models.Dtos.Users.RequestModels;
using dev_processes_backend.Models.Dtos.Users.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace dev_processes_backend.Services
{
    public class StudentsService: BaseService
    {
        public StudentsService(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public async Task<StudentResponse> GetStudent(Guid studentId)
        {
            var student = await ApplicationDbContext.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }
            return new StudentResponse {
                FirstName = student.FirstName,
                LastName = student.LastName,
                Patronymic = student.Patronymic,
                Email = student.Email,
                Course = student.Course,
                Group = student.Group,
                EducationalTrack = student.EducationalTrack
            };
        }

        public async Task<List<StudentResponse>> GetCourseStudents(int course)
        {
            var students = await ApplicationDbContext.Students.Where(s => s.Course == course).ToListAsync();
            return students.Select(s => new StudentResponse
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                Patronymic = s.Patronymic,
                Email = s.Email,
                Course = s.Course,
                Group = s.Group,
                EducationalTrack = s.EducationalTrack
            }).ToList();
        }

        public async Task<List<StudentResponse>> GetGroupStudents(string group)
        {
            var students = await ApplicationDbContext.Students.Where(s => s.Group == group).ToListAsync();
            return students.Select(s => new StudentResponse
            {
                FirstName = s.FirstName,
                LastName = s.LastName,
                Patronymic = s.Patronymic,
                Email = s.Email,
                Course = s.Course,
                Group = s.Group,
                EducationalTrack = s.EducationalTrack
            }).ToList();
        }

        public async Task EditStudent(Guid studentId, EditStudentRequest editModel)
        {
            var student = await ApplicationDbContext.Students.FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }
            student.FirstName = editModel.FirstName != null && editModel.FirstName != "" ? editModel.FirstName : student.FirstName;
            student.LastName = editModel.LastName != null && editModel.LastName != "" ? editModel.LastName : student.LastName;
            student.Patronymic = editModel.Patronymic != null && editModel.Patronymic != "" ? editModel.Patronymic : student.Patronymic;
            student.Email = editModel.Email != null && editModel.Email != "" ? editModel.Email : student.Email;
            student.Phone = editModel.Phone != null && editModel.Phone != "" ? editModel.Phone : student.Phone;
            student.Group = editModel.Group != null && editModel.Group != "" ? editModel.Group : student.Group;
            student.Course = (int)(editModel.Course != null ? editModel.Course : student.Course);
            student.EducationalTrack = (Models.EducationalTrack)(editModel.EducationalTrack != null ? editModel.EducationalTrack : student.EducationalTrack);
        }
    }
}
