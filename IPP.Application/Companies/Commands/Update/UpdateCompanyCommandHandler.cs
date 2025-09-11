using Application.Responses.Companies;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPP.Application.Companies.Commands.Update;

public class UpdateCompanyCommandHandler : ICommandHandler<UpdateCompanyCommand, CompanyResponse>
{
    private readonly DataContext _dataContext;
    public UpdateCompanyCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<CompanyResponse> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        var company = await _dataContext.Companies.FindAsync(command.Id);

        if (company == null) 
            return null;

        company.Name = command.Name;
        company.Website = command.Website;

        await _dataContext.SaveChangesAsync();

        return new CompanyResponse
        {
            Id = company.Id,
            Name = company.Name,
            Website = company.Website,
            CreateDate = company.CreateDate
        };
    }
}