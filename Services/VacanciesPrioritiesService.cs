using dev_processes_backend.Data;
using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using dev_processes_backend.Models.Dtos.VacanciesPrioroties.RequestModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace dev_processes_backend.Services
{
    public class VacanciesPrioritiesService: BaseService
    {
        public VacanciesPrioritiesService(IServiceProvider serviceProvider) : base(serviceProvider) { }
        public async Task<Guid> AddVacancyToStudentsPrioritiesList(Guid vacancyId, Guid studentId)
        {
            var student = await ApplicationDbContext.Students.Include(s => s.VacancyPriorities).FirstOrDefaultAsync(s => s.Id == studentId);
            var vacancy = await ApplicationDbContext.Vacancies.FirstOrDefaultAsync(v => v.Id == vacancyId);
            if (vacancy == null || student == null) {
                throw new EntityNotFoundException();
            }
            var studentPriorities = student.VacancyPriorities.Select(vp => vp.Value).ToList();
            var vacancyPriorityMaxValue = studentPriorities.Any() ? studentPriorities.Max() : 0;
            var vacancyPriority = new VacancyPriority
            {
                Value = vacancyPriorityMaxValue + 1,
                Vacancy = vacancy
            };
            student.VacancyPriorities.Add(vacancyPriority);
            await ApplicationDbContext.SaveChangesAsync();

            return vacancyPriority.Id;
        }

        public async Task ChangePriorities(Guid studentId, List<ChangeVacanciyPriorityRequest> vacanciesPriorities)
        {
            if (!await CheckIfVacanciesPrioritisCorrect(studentId, vacanciesPriorities))
            {
                throw new Exception();
            }
            var student = await ApplicationDbContext.Students.Include(s => s.VacancyPriorities).FirstOrDefaultAsync(s => s.Id == studentId);
            if (student == null)
            {
                throw new EntityNotFoundException();
            }
            foreach (var vacancyPriority in student.VacancyPriorities)
            {
                vacancyPriority.Value = vacanciesPriorities.FirstOrDefault(vp => vp.VacancyPriorityId == vacancyPriority.Id).Value;
            }
            await ApplicationDbContext.SaveChangesAsync();
        }

        private async Task<bool> CheckIfVacanciesPrioritisCorrect(Guid studentId, List<ChangeVacanciyPriorityRequest> vacanciesPriorities)
        {
            var student = await ApplicationDbContext.Students.Include(s => s.VacancyPriorities).FirstOrDefaultAsync(s => s.Id == studentId);

            var existsVacanciesPrioritiesIds = student.VacancyPriorities.Select(vp => vp.Id).ToList();
            var innerVacanciesPriorititesIds = vacanciesPriorities.Select(vp => vp.VacancyPriorityId).ToList();
            var innerVacanciesPrioritiesValues = vacanciesPriorities.Select(vp => vp.Value).ToList();
            foreach (var vacancyPriority in innerVacanciesPriorititesIds)
            {
                if (!existsVacanciesPrioritiesIds.Contains(vacancyPriority))
                {
                    return false;
                }
            }
            var correctPriorities = new List<int>();
            for (int i = 0; i < vacanciesPriorities.Count; i++)
            {
                correctPriorities.Add(i);
            }
            innerVacanciesPrioritiesValues.Sort();
            for (var i = 0; i < innerVacanciesPrioritiesValues.Count; i++)
            {
                if (innerVacanciesPrioritiesValues[i] != correctPriorities[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
