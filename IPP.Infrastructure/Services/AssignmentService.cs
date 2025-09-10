using Application.Commands.Assignments;
using Application.Interfaces;
using Application.Responses.Employees;
using Application.Responses.Projects;
using Domain.Entities;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class AssignmentService : IAssignmentService
{
    private readonly AppDbContext _db;

    public AssignmentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<EmployeeProjectResponse>> AssignProjectsToEmployeeAsync(AssignProjectsToEmployeeCommand command)
    {
        var employee = await _db.Employees.Include(e => e.EmployeeProjects)
                                          .FirstOrDefaultAsync(e => e.Id == command.EmployeeId);
        if (employee == null) throw new Exception("Employee not found");

        var employeeCompanyId = employee.CompanyId;

        var projects = await _db.Projects
            .Where(p => command.ProjectIds.Contains(p.Id) && p.CompanyId == employeeCompanyId)
            .ToListAsync();

        var responses = new List<EmployeeProjectResponse>();

        foreach (var project in projects)
        {
            if (employee.EmployeeProjects.Any(ep => ep.ProjectId == project.Id))
                continue;

            var assignment = new EmployeeProject
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

        await _db.SaveChangesAsync();
        return responses;
    }

    public async Task<bool> UnassignProjectFromEmployeeAsync(UnassignProjectFromEmployeeCommand command)
    {
        var assignment = await _db.EmployeesProjects
            .FirstOrDefaultAsync(ep => ep.EmployeeId == command.EmployeeId && ep.ProjectId == command.ProjectId);

        if (assignment == null) return false;

        _db.EmployeesProjects.Remove(assignment);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ProjectResponse>> GetProjectsForEmployeeAsync(GetProjectsForEmployeeQuery query)
    {
        var projects = await _db.EmployeesProjects
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

    public async Task<List<EmployeeResponse>> GetEmployeesForProjectAsync(GetEmployeesForProjectQuery query)
    {
        var employees = await _db.EmployeesProjects
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
