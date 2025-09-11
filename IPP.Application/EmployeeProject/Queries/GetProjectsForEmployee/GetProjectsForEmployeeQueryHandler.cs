using Application.Responses.Projects;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.EmployeeProject.Queries.GetProjectsForEmployee;

public class GetProjectsForEmployeeQueryHandler : IQueryHandler<GetProjectsForEmployeeQuery, List<ProjectResponse>>
{
    private readonly DataContext _dataContext;
    public GetProjectsForEmployeeQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<ProjectResponse>> Handle(GetProjectsForEmployeeQuery query, CancellationToken cancellation)
    {
        var projects = await _dataContext.EmployeesProjects
            .Where(ep => ep.EmployeeId == query.EmployeeId)
            .Select(ep => ep.Project)
            .Select(p => new ProjectResponse
            {
                Id = p.Id,
                CompanyId = p.CompanyId,
                Name = p.Name,
                StartDate = p.StartDate,
                EndDate = p.EndDate
            })
            .ToListAsync();

        return projects;
    }
}