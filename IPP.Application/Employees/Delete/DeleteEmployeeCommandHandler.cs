using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Employees.Delete;

public class DeleteEmployeeCommandHandler : ICommandHandler<DeleteEmployeeCommand, bool>
{
    private readonly IRepository<Employee> _repository;

    public DeleteEmployeeCommandHandler(IRepository<Employee> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(command.Id);

        if (employee == null)
            throw new NotFoundException($"Employee with id {command.Id} not found.");

        _repository.Remove(employee);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}