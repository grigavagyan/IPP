using Application.Commands.Assignments;
using Application.Responses.Employees;
using Application.Responses.Projects;

namespace Application.Interfaces;

public interface IAssignmentService
{
    Task<List<EmployeeProjectResponse>> AssignProjectsToEmployeeAsync(AssignProjectsToEmployeeCommand command);
    Task<bool> UnassignProjectFromEmployeeAsync(UnassignProjectFromEmployeeCommand command);
    Task<List<ProjectResponse>> GetProjectsForEmployeeAsync(GetProjectsForEmployeeQuery query);
    Task<List<EmployeeResponse>> GetEmployeesForProjectAsync(GetEmployeesForProjectQuery query);
}