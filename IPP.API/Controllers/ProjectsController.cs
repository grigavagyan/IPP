using Application.Responses.Common;
using Application.Responses.Projects;
using IPP.Application.Projects.Create;
using IPP.Application.Projects.Delete;
using IPP.Application.Projects.GetProjectById;
using IPP.Application.Projects.GetProjects;
using IPP.Application.Projects.Interfaces;
using IPP.Application.Projects.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[Route("[controller]")]
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
    public async Task<ActionResult<PagedResponse<ProjectResponse>>> Get(
        [FromQuery] GetProjectsQuery query)
    {
        var response = await _queryDispatcher.Dispatch<GetProjectsQuery, ProjectResponse>(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ProjectResponse>> GetById(
        [Required] Guid id)
    {
        var response = await _queryDispatcher.Dispatch<GetProjectByIdQuery, ProjectResponse>(new GetProjectByIdQuery { Id = id });
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProjectResponse>> Create(
        [FromBody] CreateProjectCommand command)
    {
        var response = await _commandDispatcher.Dispatch<CreateProjectCommand, ProjectResponse>(command);
        return Ok(response);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProjectResponse>> Update(
        [FromBody] UpdateProjectCommand command)
    {
        var response = await _commandDispatcher.Dispatch<UpdateProjectCommand, ProjectResponse>(command);
        return Ok(response);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
        [Required] Guid id)
    {
        var response = await _commandDispatcher.Dispatch<DeleteProjectCommand, bool>(new DeleteProjectCommand { Id = id });
        if (!response) return NotFound();
        return Ok(response);
    }
}