using Application.Responses.Common;
using Application.Responses.Companies;
using IPP.Application.Employees.EmployeeProject.Companies.Create;
using IPP.Application.Employees.EmployeeProject.Companies.Delete;
using IPP.Application.Employees.EmployeeProject.Companies.GetCompanies;
using IPP.Application.Employees.EmployeeProject.Companies.GetCompanyById;
using IPP.Application.Employees.EmployeeProject.Companies.Update;
using IPP.Application.Projects.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IPP.API.Controllers;

[ApiController]
[Route("[controller]")]
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
    public async Task<ActionResult<PagedResponse<CompanyResponse>>> Get(
        [FromQuery] GetCompaniesQuery query)
    {
        var response = await _queryDispatcher.Dispatch<GetCompaniesQuery, PagedResponse<CompanyResponse>>(query);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<CompanyResponse>> GetById(
        [Required] Guid id)
    {
        var response = await _queryDispatcher.Dispatch<GetCompanyByIdQuery, CompanyResponse>(new GetCompanyByIdQuery { Id = id});
        return Ok(response); //ToDo check null in handler
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateCompanyCommand command)
    {
        _ = await _commandDispatcher.Dispatch<CreateCompanyCommand, CompanyResponse>(command);
        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        [FromBody] UpdateCompanyCommand command)
    {
        _ = await _commandDispatcher.Dispatch<UpdateCompanyCommand, CompanyResponse>(command);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
        DeleteCompanyCommand command)
    {
        _ = await _commandDispatcher.Dispatch<DeleteCompanyCommand, bool>(command);
        return Ok();
    }
}