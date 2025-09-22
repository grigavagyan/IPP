using Application.Responses.Employees;
using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Employees.Update;

public class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand, EmployeeResponse>
{
    private readonly IRepository<Employee> _repository;

    public UpdateEmployeeCommandHandler(IRepository<Employee> repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeResponse> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(command.Id);

        if (employee == null)
            throw new NotFoundException($"Company with id {command.Id} not found.");

        employee.FirstName = command.FirstName;
        employee.LastName = command.LastName;
        employee.Email = command.Email;
        employee.HireDate = command.HireDate;
        employee.CompanyId = command.CompanyId;

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