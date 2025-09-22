using Application.Responses.Employees;
using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Employees.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IQueryHandler<GetEmployeeByIdQuery, EmployeeResponse>
{
    private readonly IRepository<Employee> _repository;

    public GetEmployeeByIdQueryHandler(IRepository<Employee> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeResponse> Handle(GetEmployeeByIdQuery query, CancellationToken cancellation)
    {
        var employee = await _repository.GetByIdAsync(query.Id);

        if (employee == null)
            throw new NotFoundException($"Employee with id {query.Id} not found.");

        return new EmployeeResponse
        {
            Id = employee.Id,
            CompanyId = employee.CompanyId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            HireDate = employee.HireDate
        };
    }
}