using Application.Responses.Employees;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;

namespace IPP.Application.Employees.Commands.Update;

public class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand, EmployeeResponse>
{
    private readonly DataContext _dataContext;

    public UpdateEmployeeCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<EmployeeResponse> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _dataContext.Employees.FindAsync(command.Id);
        if (employee == null) return null;

        employee.FirstName = command.FirstName;
        employee.LastName = command.LastName;
        employee.Email = command.Email;
        employee.HireDate = command.HireDate;
        employee.CompanyId = command.CompanyId;

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