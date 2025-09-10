using Application.Commands.Companies;
using Application.Interfaces;
using Application.Queries.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IPP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _service;

    public CompaniesController(ICompanyService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetCompaniesQuery query)
        => Ok(await _service.GetCompaniesAsync(query));

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var company = await _service.GetCompanyByIdAsync(new GetCompanyByIdQuery { Id = id });
        return company == null ? NotFound() : Ok(company);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyCommand command)
    {
        var result = await _service.CreateCompanyAsync(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyCommand command)
    {
        command.Id = id;
        var result = await _service.UpdateCompanyAsync(command);
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, [FromQuery] bool force = false)
    {
        var result = await _service.DeleteCompanyAsync(new DeleteCompanyCommand { Id = id, Force = force });
        return result ? NoContent() : NotFound();
    }
}
