namespace dev_processes_backend.Models.Dtos.Users.ResponseModels;

public class GetAdminsElementResponseModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string Email { get; set; }
    public DateTime CreateDateTime { get; set; }
    public bool IsActive { get; set; }
}