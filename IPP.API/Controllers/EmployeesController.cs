using Application.Responses.Common;
using Application.Responses.Employees;
using IPP.Application.Employees.Delete;
using IPP.Application.Employees.GetEmployees;
using IPP.Application.Employees.Update;
using IPP.Application.Projects.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    public EmployeesController(
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
        )
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResponse<EmployeeResponse>>> Get(
        [FromQuery] GetEmployeesQuery query)
    {
        var response = await _queryDispatcher.Dispatch<GetEmployeesQuery, PagedResponse<EmployeeResponse>>(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<EmployeeResponse>> GetById(
        [Required] Guid id)
    {
        var response = await _queryDispatcher.Dispatch<GetEmployeeByIdQuery, EmployeeResponse>(new GetEmployeeByIdQuery { Id = id });
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateEmployeeCommand command)
    {
        _ = await _commandDispatcher.Dispatch<CreateEmployeeCommand, EmployeeResponse>(command);
        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateEmployeeCommand command)
    {
        _ = await _commandDispatcher.Dispatch<UpdateEmployeeCommand, EmployeeResponse>(command);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
        [Required] Guid id)
    {
        var command = new DeleteEmployeeCommand { Id = id };

        _ = await _commandDispatcher.Dispatch<DeleteEmployeeCommand, bool>(command);
        return Ok();
    }
}