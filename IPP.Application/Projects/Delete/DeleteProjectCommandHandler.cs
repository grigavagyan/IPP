using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Projects.Delete;

public class DeleteProjectCommandHandler : ICommandHandler<DeleteProjectCommand, bool>
{
    private readonly IRepository<Project> _repository;
    public DeleteProjectCommandHandler(IRepository<Project> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(command.Id);

        if (project == null)
            throw new NotFoundException($"Project with id {command.Id} not found.");

        _repository.Remove(project);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}