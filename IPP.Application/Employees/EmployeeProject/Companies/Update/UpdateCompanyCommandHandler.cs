using Application.Responses.Companies;
using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Employees.EmployeeProject.Companies.Update;

public class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand, CompanyResponse>
{
    private readonly IRepository<Company> _repository;

    public UpdateCompanyCommandHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }
    public async Task<CompanyResponse> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        var company = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (company == null)
            throw new NotFoundException($"Company with id {command.Id} not found.");

        company.Name = command.Name;
        company.Website = command.Website;

        _repository.Update(company);
        await _repository.SaveChangesAsync(cancellationToken);

        return new CompanyResponse
        {
            Id = company.Id,
            Name = company.Name,
            Website = company.Website,
            CreateDate = company.CreateDate
        };
    }
}