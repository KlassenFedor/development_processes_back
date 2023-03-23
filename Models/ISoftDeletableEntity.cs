namespace dev_processes_backend.Models;

public interface ISoftDeletableEntity
{
    public bool IsDeleted { get; set; }
}