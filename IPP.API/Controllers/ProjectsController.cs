using Application.Interfaces;
using Application.Projects.Commands;
using Application.Projects.Queries;
using Application.Responses.Common;
using Application.Responses.Employees;
using Application.Responses.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IAssignmentService _assignmentService;

    public ProjectsController(IProjectService projectService, IAssignmentService assignmentService)
    {
        _projectService = projectService;
        _assignmentService = assignmentService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResponse<ProjectResponse>>> Get([FromQuery] GetProjectsQuery query)
    {
        var projects = await _projectService.GetProjectsAsync(query);
        return Ok(projects);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
    {
        var project = await _projectService.GetProjectByIdAsync(new GetProjectByIdQuery { Id = id });
        if (project == null) return NotFound();
        return Ok(project);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create(CreateProjectCommand command)
    {
        var project = await _projectService.CreateProjectAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ProjectResponse>> Update(Guid id, UpdateProjectCommand command)
    {
        if (id != command.Id) return BadRequest();
        var project = await _projectService.UpdateProjectAsync(command);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _projectService.DeleteProjectAsync(new DeleteProjectCommand { Id = id });
        if (!deleted) return NotFound();
        return NoContent();
    }

    [Authorize]
    [HttpGet("{projectId}/employees")]
    public async Task<ActionResult<List<EmployeeResponse>>> GetEmployeesForProject(Guid projectId)
    {
        var result = await _assignmentService.GetEmployeesForProjectAsync(
            new GetEmployeesForProjectQuery { ProjectId = projectId });
        return Ok(result);
    }
}