using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;

namespace IPP.Application.Employees.Commands.Delete;

public class DeleteEmployeeCommandHandler : ICommandHandler<DeleteEmployeeCommand, bool>
{
    private readonly DataContext _dataContext;

    public DeleteEmployeeCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _dataContext.Employees.FindAsync(command.Id);
        if (employee == null) return false;

        _dataContext.Employees.Remove(employee);
        await _dataContext.SaveChangesAsync();
        return true;
    }
}