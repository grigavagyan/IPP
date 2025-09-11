using Application.Responses.Employees;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.EmployeeProject.Queries.GetEmployeesForProject;

public class GetEmployeesForProjectQueryHandler : IQueryHandler<GetEmployeesForProjectQuery, List<EmployeeResponse>>
{
    private readonly DataContext _dataContext;
    public GetEmployeesForProjectQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<EmployeeResponse>> Handle(GetEmployeesForProjectQuery query, CancellationToken cancellation)
    {
        var employees = await _dataContext.EmployeesProjects
            .Where(ep => ep.ProjectId == query.ProjectId)
            .Select(ep => ep.Employee)
            .Select(e => new EmployeeResponse
            {
                Id = e.Id,
                CompanyId = e.CompanyId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                HireDate = e.HireDate
            })
            .ToListAsync();

        return employees;
    }
}