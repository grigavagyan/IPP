using Application.Responses.Companies;
using Domain.Entities;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Employees.EmployeeProject.Companies.Create;

public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, CompanyResponse>
{
    private readonly IRepository<Company> _repository;

    public CreateCompanyCommandHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<CompanyResponse> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
    {
        var newCompany = new Company
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Website = command.Website
        };

        await _repository.AddAsync(newCompany);
        await _repository.SaveChangesAsync(cancellationToken);

        return new CompanyResponse
        {
            Id = newCompany.Id,
            Name = newCompany.Name,
            Website = newCompany.Website,
            CreateDate = newCompany.CreateDate
        };
    }
}