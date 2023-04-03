using dev_processes_backend.Exceptions;
using dev_processes_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace dev_processes_backend.Services;

public class InterviewsService : BaseService
{
    public InterviewsService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public async Task ConfirmOfferAsync(Guid? id)
    {
        if (id == null)
        {
            throw new EntityNotFoundException();
        }
        var interview = await ApplicationDbContext.Interviews
            .Include(i => i.History)
            .FirstOrDefaultAsync(i => i.Id == id);
        if (interview == null)
        {
            throw new EntityNotFoundException();
        }
        
        interview.History.Add(new InterviewState
        {
            Id = new Guid(),
            DateTime = DateTime.Now,
            Status = InterviewStatus.OfferConfirmed
        });
        
        ApplicationDbContext.Interviews.Update(interview);
        await ApplicationDbContext.SaveChangesAsync();
    }
}