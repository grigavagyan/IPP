using Application.Responses.Projects;
using Domain.Entities;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;

namespace IPP.Application.Projects.Commands.Create;

public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, ProjectResponse>
{
    private readonly DataContext _dataContext;
    public CreateProjectCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
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

        _dataContext.Projects.Add(project);
        await _dataContext.SaveChangesAsync();

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