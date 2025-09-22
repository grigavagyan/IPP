using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Employees.EmployeeProject.AssignProjectsToEmployee;

public class AssignProjectsToEmployeeCommandHandler : ICommandHandler<AssignProjectsToEmployeeCommand, List<EmployeeProjectResponse>>
{
    private readonly IRepository<Project> _repository;
    public AssignProjectsToEmployeeCommandHandler(IRepository<Project> repository)
    {
        _repository = repository;
    }

    public async Task<List<EmployeeProjectResponse>> Handle(AssignProjectsToEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _repository.Query()
            .Include(p => p.EmployeeProjects)
            .FirstOrDefaultAsync(e => e.Id == command.EmployeeId);

        if (employee == null)
            throw new NotFoundException($"Employee with id {command.EmployeeId} not found.");

        var employeeCompanyId = employee.CompanyId;

        var projects = await _repository.Query()
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

        await _repository.SaveChangesAsync(cancellationToken);
        return responses;
    }
}