using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;

namespace IPP.Application.Projects.Commands.Delete;

public class DeleteProjectCommandHandler : ICommandHandler<DeleteProjectCommand, bool>
{
    private readonly DataContext _dataContext;
    public DeleteProjectCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<bool> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        var project = await _dataContext.Projects.FindAsync(command.Id);
        if (project == null) return false;

        _dataContext.Projects.Remove(project);
        await _dataContext.SaveChangesAsync();
        return true;
    }
}