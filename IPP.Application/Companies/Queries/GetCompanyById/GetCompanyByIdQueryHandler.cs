using Application.Responses.Companies;
using Domain.Entities;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPP.Application.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, CompanyResponse>
{
    private readonly DataContext _dataContext;
    public GetCompanyByIdQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<CompanyResponse> Handle(GetCompanyByIdQuery query, CancellationToken cancellation)
    {
        var company = await _dataContext.Companies.FindAsync(query.Id);

        return company == null ? null : new CompanyResponse
        {
            Id = company.Id,
            Name = company.Name,
            Website = company.Website,
            CreateDate = company.CreateDate
        };
    }
}