namespace dev_processes_backend.Models.Dtos.VacanciesPrioroties.RequestModels
{
    public class ChangeVacanciyPriorityRequest
    {
        public Guid VacancyPriorityId { get; set; }
        public int Value { get; set; }
    }
}
