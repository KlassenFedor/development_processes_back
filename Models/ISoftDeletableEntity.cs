namespace dev_processes_backend.Models;

public interface ISoftDeletableEntity
{
    public DateTime CreateDateTime { get; set; }
    public bool IsDeleted { get; set; }
}