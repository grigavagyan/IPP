using Application.Responses.Projects;
using IPP.Application.Projects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Employees.EmployeeProject.GetProjectsForEmployee;

public class GetProjectsForEmployeeQueryHandler : IQueryHandler<GetProjectsForEmployeeQuery, List<ProjectResponse>>
{
    private readonly IRepository<Domain.Entities.EmployeeProject> _repository;
    public GetProjectsForEmployeeQueryHandler(IRepository<Domain.Entities.EmployeeProject> repository)
    {
        _repository = repository;
    }

    public async Task<List<ProjectResponse>> Handle(GetProjectsForEmployeeQuery query, CancellationToken cancellation)
    {
        var projects = await _repository.Query()
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