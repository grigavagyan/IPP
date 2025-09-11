using Application.Responses.Common;
using Application.Responses.Employees;
using IPP.Application.Employees.Commands.Delete;
using IPP.Application.Employees.Commands.Update;
using IPP.Application.Employees.Queries.GetEmployees;
using IPP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> Get([FromQuery] GetEmployeesQuery query)
    {
        var response = await _queryDispatcher.Dispatch<GetEmployeesQuery, PagedResponse<EmployeeResponse>>(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var emp = await _queryDispatcher.Dispatch<GetEmployeeByIdQuery, EmployeeResponse>(new GetEmployeeByIdQuery { Id = id });
        return emp == null ? NotFound() : Ok(emp);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
    {
        var response = await _commandDispatcher.Dispatch<CreateEmployeeCommand, EmployeeResponse>(command);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] UpdateEmployeeCommand command)
    {
        var response = await _commandDispatcher.Dispatch<UpdateEmployeeCommand, EmployeeResponse>(command);
        return response == null ? NotFound() : Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteEmployeeCommand { Id = id };

        var response = await _commandDispatcher.Dispatch<DeleteEmployeeCommand, bool>(command);
        return response ? NoContent() : NotFound();
    }
}