using Application.Responses.Employees;
using IPP.Application.Projects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Employees.EmployeeProject.GetEmployeesForProject;

public class GetEmployeesForProjectQueryHandler : IQueryHandler<GetEmployeesForProjectQuery, List<EmployeeResponse>>
{
    private readonly IRepository<Domain.Entities.EmployeeProject> _repository;
    public GetEmployeesForProjectQueryHandler(IRepository<Domain.Entities.EmployeeProject> repository)
    {
        _repository = repository;
    }

    public async Task<List<EmployeeResponse>> Handle(GetEmployeesForProjectQuery query, CancellationToken cancellation)
    {
        var employees = await _repository.Query()
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