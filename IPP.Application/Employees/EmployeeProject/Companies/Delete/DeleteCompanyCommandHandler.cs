using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Employees.EmployeeProject.Companies.Delete;

public class DeleteCompanyCommandHandler : ICommandHandler<DeleteCompanyCommand, bool>
{
    private readonly IRepository<Company> _repository;

    public DeleteCompanyCommandHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
    {
        var company = await _repository.Query()
            .Include(c => c.Employees)
            .Include(c => c.Projects)
            .FirstOrDefaultAsync(c => c.Id == command.Id);

        if (company == null)
            throw new NotFoundException($"Company with id {command.Id} not found.");

        if (company.Employees.Any() || company.Projects.Any())
        {
            throw new InvalidOperationException("Company has employees or projects.");
        }

        _repository.Remove(company);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}