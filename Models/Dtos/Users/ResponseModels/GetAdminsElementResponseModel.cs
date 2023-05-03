namespace dev_processes_backend.Models.Dtos.Users.ResponseModels;

public class GetAdminsElementResponseModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string Email { get; set; }
    public DateTime CreateDateTime { get; set; }
    public string Status { get; set; }
    /// <summary>
    /// Used for setting the string value of <see cref="Status"/>
    /// </summary>
    public bool IsActive
    {
        set => Status = value ? "active" : "blocked";
    }
}