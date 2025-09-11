using Application.Responses.Common;
using Application.Responses.Projects;
using IPP.Application.Interfaces;
using IPP.Application.Projects.Commands.Create;
using IPP.Application.Projects.Commands.Delete;
using IPP.Application.Projects.Commands.Update;
using IPP.Application.Projects.Queries.GetProjectById;
using IPP.Application.Projects.Queries.GetProjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public ProjectsController(
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
        )
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResponse<ProjectResponse>>> Get([FromQuery] GetProjectsQuery query)
    {
        var response = await _queryDispatcher.Dispatch<GetProjectsQuery, ProjectResponse>(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
    {
        var response = await _queryDispatcher.Dispatch<GetProjectByIdQuery, ProjectResponse>(new GetProjectByIdQuery { Id = id });
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProjectResponse>> Create([FromBody] CreateProjectCommand command)
    {
        var response = await _commandDispatcher.Dispatch<CreateProjectCommand, ProjectResponse>(command);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProjectResponse>> Update([FromBody] UpdateProjectCommand command)
    {
        var response = await _commandDispatcher.Dispatch<UpdateProjectCommand, ProjectResponse>(command);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _commandDispatcher.Dispatch<DeleteProjectCommand, bool>(new DeleteProjectCommand { Id = id });
        if (!response) return NotFound();
        return Ok(response);
    }
}