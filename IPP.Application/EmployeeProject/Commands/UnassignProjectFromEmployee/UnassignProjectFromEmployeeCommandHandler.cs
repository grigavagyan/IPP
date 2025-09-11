using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.EmployeeProject.Commands.UnassignProjectFromEmployee;

public class UnassignProjectFromEmployeeCommandHandler : ICommandHandler<UnassignProjectFromEmployeeCommand, bool>
{
    private readonly DataContext _dataContext;
    public UnassignProjectFromEmployeeCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> Handle(UnassignProjectFromEmployeeCommand command, CancellationToken cancellationToken)
    {
        var assignment = await _dataContext.EmployeesProjects
            .FirstOrDefaultAsync(ep => ep.EmployeeId == command.EmployeeId && ep.ProjectId == command.ProjectId);

        if (assignment == null) return false;

        _dataContext.EmployeesProjects.Remove(assignment);
        await _dataContext.SaveChangesAsync();
        return true;
    }
}