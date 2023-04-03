using dev_processes_backend.Data;

namespace dev_processes_backend.Services;

public class BaseService
{
    protected readonly ApplicationDbContext ApplicationDbContext;

    protected BaseService(IServiceProvider serviceProvider)
    {
        ApplicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
    }
}