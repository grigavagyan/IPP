using Application.Responses.Employees;
using Application.Responses.Projects;
using IPP.Application.Employees.EmployeeProject.AssignProjectsToEmployee;
using IPP.Application.Projects.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IPP.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AssignmentController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    public AssignmentController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet("projects/{projectId}/employees")]
    [Authorize]
    public async Task<ActionResult<List<EmployeeResponse>>> GetEmployeesForProject(
        [Required] Guid projectId)
    {
        var result = await _queryDispatcher.Dispatch<GetEmployeesForProjectQuery, List<EmployeeResponse>>(
            new GetEmployeesForProjectQuery { ProjectId = projectId });
        return Ok(result);
    }

    [HttpGet("employees/{employeeId}/projects")]
    [Authorize]
    public async Task<ActionResult<List<ProjectResponse>>> GetProjectsForEmployee(
        [Required] Guid employeeId)
    {
        var response = await _queryDispatcher.Dispatch<GetProjectsForEmployeeQuery, List<ProjectResponse>>(
            new GetProjectsForEmployeeQuery { EmployeeId = employeeId });
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<EmployeeProjectResponse>>> AssignProjects(
        [Required] Guid employeeId,
        [FromBody] List<Guid> projectIds)
    {
        var response = await _commandDispatcher.Dispatch<AssignProjectsToEmployeeCommand, List<EmployeeProjectResponse>>(
            new AssignProjectsToEmployeeCommand { EmployeeId = employeeId, ProjectIds = projectIds });

        return Ok(response);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UnassignProject(
        [Required] Guid employeeId,
        [Required] Guid projectId)
    {
        var response = await _commandDispatcher.Dispatch<UnassignProjectFromEmployeeCommand, bool>(
            new UnassignProjectFromEmployeeCommand { EmployeeId = employeeId, ProjectId = projectId });

        return response ? NoContent() : NotFound();
    }
}