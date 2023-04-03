namespace dev_processes_backend.Models;

public abstract class DownloadableDocument
{
    public Guid Id { get; set; }
    /// <summary>
    /// Первый календарный год учебного года, в течение которого документ опубликован.
    /// Например, 2022 для 2022-2023 учебного года
    /// </summary>
    public int StudyYearStart { get; set; }
    public int Version { get; set; }
    
    public File File { get; set; }
}