using Application.Responses.Employees;
using Application.Responses.Projects;
using IPP.Application.EmployeeProject.Commands.AssignProjectsToEmployee;
using IPP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IPP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssignmentController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    public AssignmentController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpPost("{employeeId}/projects")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<EmployeeProjectResponse>>> AssignProjects(
        Guid employeeId,
        [FromBody] List<Guid> projectIds
        )
    {
        var response = await _commandDispatcher.Dispatch<AssignProjectsToEmployeeCommand, List<EmployeeProjectResponse>>(
            new AssignProjectsToEmployeeCommand { EmployeeId = employeeId, ProjectIds = projectIds });

        return Ok(response);
    }

    [HttpDelete("{employeeId}/projects/{projectId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UnassignProject(Guid employeeId, Guid projectId)
    {
        var response = await _commandDispatcher.Dispatch<UnassignProjectFromEmployeeCommand, bool>(
            new UnassignProjectFromEmployeeCommand { EmployeeId = employeeId, ProjectId = projectId });

        return response ? NoContent() : NotFound();
    }

    [HttpGet("{projectId}/employees")]
    [Authorize]
    public async Task<ActionResult<List<EmployeeResponse>>> GetEmployeesForProject(Guid projectId)
    {
        var result = await _queryDispatcher.Dispatch<GetEmployeesForProjectQuery, List<EmployeeResponse>>(
            new GetEmployeesForProjectQuery { ProjectId = projectId });
        return Ok(result);
    }

    [HttpGet("{employeeId}/projects")]
    [Authorize]
    public async Task<ActionResult<List<ProjectResponse>>> GetProjectsForEmployee(Guid employeeId)
    {
        var response = await _queryDispatcher.Dispatch<GetProjectsForEmployeeQuery, List<ProjectResponse>>(
            new GetProjectsForEmployeeQuery { EmployeeId = employeeId });
        return Ok(response);
    }
}