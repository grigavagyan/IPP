using Application.Commands.Assignments;
using Application.Commands.Employees;
using Application.Interfaces;
using Application.Queries.Employees;
using Application.Responses.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IAssignmentService _assignmentService;
    public EmployeesController(IEmployeeService employeeService, IAssignmentService assignmentService)
    {
        _employeeService = employeeService;
        _assignmentService = assignmentService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetEmployeesQuery query)
        => Ok(await _employeeService.GetEmployeesAsync(query));

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var emp = await _employeeService.GetEmployeeByIdAsync(new GetEmployeeByIdQuery { Id = id });
        return emp == null ? NotFound() : Ok(emp);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
    {
        var emp = await _employeeService.CreateEmployeeAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = emp.Id }, emp);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeCommand command)
    {
        command.Id = id;
        var emp = await _employeeService.UpdateEmployeeAsync(command);
        return emp == null ? NotFound() : Ok(emp);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _employeeService.DeleteEmployeeAsync(new DeleteEmployeeCommand { Id = id });
        return success ? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{employeeId}/projects")]
    public async Task<ActionResult<List<EmployeeProjectResponse>>> AssignProjects(
    Guid employeeId,
    [FromBody] List<Guid> projectIds)
    {
        var result = await _assignmentService.AssignProjectsToEmployeeAsync(
            new AssignProjectsToEmployeeCommand { EmployeeId = employeeId, ProjectIds = projectIds });
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{employeeId}/projects/{projectId}")]
    public async Task<IActionResult> UnassignProject(Guid employeeId, Guid projectId)
    {
        var result = await _assignmentService.UnassignProjectFromEmployeeAsync(
            new UnassignProjectFromEmployeeCommand { EmployeeId = employeeId, ProjectId = projectId });

        return result ? NoContent() : NotFound();
    }

    [Authorize]
    [HttpGet("{employeeId}/projects")]
    public async Task<ActionResult<List<ProjectResponse>>> GetProjectsForEmployee(Guid employeeId)
    {
        var result = await _assignmentService.GetProjectsForEmployeeAsync(
            new GetProjectsForEmployeeQuery { EmployeeId = employeeId });
        return Ok(result);
    }
}
