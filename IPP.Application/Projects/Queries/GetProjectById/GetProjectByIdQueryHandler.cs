using Application.Responses.Projects;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;

namespace IPP.Application.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
{
    private readonly DataContext _dataContext;
    public GetProjectByIdQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<ProjectResponse> Handle(GetProjectByIdQuery query, CancellationToken cancellation)
    {
        var project = await _dataContext.Projects.FindAsync(query.Id);
        if (project == null) return null;

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