using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Companies.Commands.Delete;

public class DeleteCompanyCommandHandler : ICommandHandler<DeleteCompanyCommand, bool>
{
    private readonly DataContext _dataContext;
    public DeleteCompanyCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<bool> Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
    {
        var company = await _dataContext.Companies
            .Include(c => c.Employees)
            .Include(c => c.Projects)
            .FirstOrDefaultAsync(c => c.Id == command.Id);

        if (company == null) return false;

        if (!command.Force && (company.Employees.Any() || company.Projects.Any()))
        {
            throw new InvalidOperationException("Company has employees or projects. Use force=true to delete.");
        }

        _dataContext.Companies.Remove(company);
        await _dataContext.SaveChangesAsync();
        return true;
    }
}
