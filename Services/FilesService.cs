using File = dev_processes_backend.Models.File;

namespace dev_processes_backend.Services;

public class FilesService : BaseService
{
    private readonly string _uploadsLocation;
    
    public FilesService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
    {
        _uploadsLocation = configuration["UploadsLocation"] ?? 
                           throw new ArgumentNullException(message: "Не указана директория для загрузки файлов", null);
    }

    public async Task<File> SaveFileAsync(IFormFile formFile)
    {
        try
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), _uploadsLocation);
            if (!Path.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var newName = Path.GetRandomFileName();
            var path = Path.Combine(directory, newName);
            var extension = Path.GetExtension(formFile.FileName);
            
            var stream = new FileStream(path, FileMode.Create);
            await formFile.CopyToAsync(stream);
            var fileEntity = new File
            {
                Id = new Guid(),
                Name = newName,
                Path = path,
                Extension = extension
            };
            return fileEntity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}