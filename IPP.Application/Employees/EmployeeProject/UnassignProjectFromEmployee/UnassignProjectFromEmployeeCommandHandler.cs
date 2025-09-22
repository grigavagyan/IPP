using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Employees.EmployeeProject.UnassignProjectFromEmployee;

public class UnassignProjectFromEmployeeCommandHandler : ICommandHandler<UnassignProjectFromEmployeeCommand, bool>
{
    private readonly IRepository<Domain.Entities.EmployeeProject> _repository;
    public UnassignProjectFromEmployeeCommandHandler(IRepository<Domain.Entities.EmployeeProject> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UnassignProjectFromEmployeeCommand command, CancellationToken cancellationToken)
    {
        var assignment = await _repository.Query()
            .FirstOrDefaultAsync(ep => ep.EmployeeId == command.EmployeeId && ep.ProjectId == command.ProjectId);

        if (assignment == null)
            throw new NotFoundException($"Assignment with id {command.EmployeeId} and {command.ProjectId} not found.");

        _repository.Remove(assignment);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}