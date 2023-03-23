namespace dev_processes_backend.Models;

public class Practice
{
    public Guid Id { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public int Course { get; set; }
    public int CharacterizationMark { get; set; }
    
    public Guid PracticeDiaryId { get; set; }
    public File PracticeDiary { get; set; }
    public Guid CharacterizationFileId { get; set; }
    public File CharacterizationFile { get; set; }
}