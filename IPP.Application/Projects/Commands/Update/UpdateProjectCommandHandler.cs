using Application.Responses.Projects;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;

namespace IPP.Application.Projects.Commands.Update;

public class UpdateProjectCommandHandler : ICommandHandler<UpdateProjectCommand, ProjectResponse>
{
    private readonly DataContext _dataContext;
    public UpdateProjectCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<ProjectResponse> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = await _dataContext.Projects.FindAsync(command.Id);
        if (project == null) return null;

        project.CompanyId = command.CompanyId;
        project.Name = command.Name;
        project.StartDate = command.StartDate;
        project.EndDate = command.EndDate;

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