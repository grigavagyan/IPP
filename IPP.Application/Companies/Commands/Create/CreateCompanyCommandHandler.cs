using Application.Responses.Companies;
using Domain.Entities;
using IPP.Application.Companies.Commands.Create;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;

namespace IPP.Infrastructure.Services;

public class CreateCompanyCommandHandler : ICommandHandler<CreateCompanyCommand, CompanyResponse>
{
    private readonly DataContext _dataContext;

    public CreateCompanyCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<CompanyResponse> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
    {
        var newCompany = new Company
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Website = command.Website
        };

        _dataContext.Companies.Add(newCompany);
        await _dataContext.SaveChangesAsync();

        return new CompanyResponse
        {
            Id = newCompany.Id,
            Name = newCompany.Name,
            Website = newCompany.Website,
            CreateDate = newCompany.CreateDate
        };
    }
}
