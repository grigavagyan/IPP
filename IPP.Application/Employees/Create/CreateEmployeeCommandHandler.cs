using Application.Responses.Employees;
using Domain.Entities;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Employees.Create;

public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, EmployeeResponse>
{
    private readonly IRepository<Employee> _repository;

    public CreateEmployeeCommandHandler(IRepository<Employee> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeResponse> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            HireDate = command.HireDate
        };

        await _repository.AddAsync(employee);
        await _repository.SaveChangesAsync(cancellationToken);

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