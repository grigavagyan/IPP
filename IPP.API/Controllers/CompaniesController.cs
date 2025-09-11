using Application.Responses.Common;
using Application.Responses.Companies;
using IPP.Application.Companies.Commands.Create;
using IPP.Application.Companies.Commands.Delete;
using IPP.Application.Companies.Commands.Update;
using IPP.Application.Companies.Queries.GetCompanies;
using IPP.Application.Companies.Queries.GetCompanyById;
using IPP.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IPP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CompaniesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] GetCompaniesQuery query)
    {
        var response = await _queryDispatcher.Dispatch<GetCompaniesQuery, PagedResponse<CompanyResponse>>(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var response = await _queryDispatcher.Dispatch<GetCompanyByIdQuery, CompanyResponse>(new GetCompanyByIdQuery { Id = id});
        return response == null ? NotFound() : Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
    {
        var response = await _commandDispatcher.Dispatch<CreateCompanyCommand, CompanyResponse>(command);
        return Ok(response);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyCommand command)
    {
        command.Id = id;
        var response = await _commandDispatcher.Dispatch<UpdateCompanyCommand, CompanyResponse>(command);
        return response == null ? NotFound() : Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] bool force = false)
    {
        var command = new DeleteCompanyCommand
        {
            Id = id,
            Force = force
        };

        var response = await _commandDispatcher.Dispatch<DeleteCompanyCommand, bool>(command);
        return Ok(response);
    }
}
