namespace dev_processes_backend.Models;

public class File
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Extension { get; set; }
}