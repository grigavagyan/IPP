using Application.Responses.Employees;
using Domain.Entities;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;

namespace IPP.Application.Employees.Commands.Create;

public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, EmployeeResponse>
{
    private readonly DataContext _dataContext;

    public CreateEmployeeCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
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

        _dataContext.Employees.Add(employee);
        await _dataContext.SaveChangesAsync();

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
