using Application.Responses.Projects;
using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Projects.Update;

public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand, ProjectResponse>
{
    private readonly IRepository<Project> _repository;
    public UpdateProjectCommandHandler(IRepository<Project> repository)
    {
        _repository = repository;
    }

    public async Task<ProjectResponse> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(command.Id);

        if (project == null)
            throw new NotFoundException($"Project with id {command.Id} not found.");

        project.CompanyId = command.CompanyId;
        project.Name = command.Name;
        project.StartDate = command.StartDate;
        project.EndDate = command.EndDate;

        await _repository.SaveChangesAsync(cancellationToken);

        return new ProjectResponse
        {
            Id = project.Id,
            CompanyId = project.CompanyId,
            Name = project.Name,
            StartDate = project.StartDate,
            EndDate = project.EndDate
        };
    }
}