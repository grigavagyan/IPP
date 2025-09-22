using Application.Responses.Projects;
using Domain.Entities;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Projects.Create;

public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, ProjectResponse>
{
    private readonly IRepository<Project> _repository;
    public CreateProjectCommandHandler(IRepository<Project> repository)
    {
        _repository = repository;
    }

    public async Task<ProjectResponse> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            Name = command.Name,
            StartDate = command.StartDate,
            EndDate = command.EndDate
        };

        await _repository.AddAsync(project);
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