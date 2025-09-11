using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace IPP.Application.EmployeeProject.Commands.AssignProjectsToEmployee;

public class AssignProjectsToEmployeeCommandHandler : ICommandHandler<AssignProjectsToEmployeeCommand, List<EmployeeProjectResponse>>
{
    private readonly DataContext _dataContext;
    public AssignProjectsToEmployeeCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<EmployeeProjectResponse>> Handle(AssignProjectsToEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _dataContext.Employees.Include(e => e.EmployeeProjects)
                                          .FirstOrDefaultAsync(e => e.Id == command.EmployeeId);
        if (employee == null) throw new Exception("Employee not found");

        var employeeCompanyId = employee.CompanyId;

        var projects = await _dataContext.Projects
            .Where(p => command.ProjectIds.Contains(p.Id) && p.CompanyId == employeeCompanyId)
            .ToListAsync();

        var responses = new List<EmployeeProjectResponse>();

        foreach (var project in projects)
        {
            if (employee.EmployeeProjects.Any(ep => ep.ProjectId == project.Id))
                continue;

            var assignment = new Domain.Entities.EmployeeProject
            {
                EmployeeId = employee.Id,
                ProjectId = project.Id,
                AssignedDate = DateTime.UtcNow
            };
            employee.EmployeeProjects.Add(assignment);

            responses.Add(new EmployeeProjectResponse
            {
                EmployeeId = employee.Id,
                ProjectId = project.Id,
                AssignedDate = assignment.AssignedDate
            });
        }

        await _dataContext.SaveChangesAsync();
        return responses;
    }
}