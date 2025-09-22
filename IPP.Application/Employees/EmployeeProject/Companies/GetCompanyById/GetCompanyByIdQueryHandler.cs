using Application.Responses.Companies;
using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Employees.EmployeeProject.Companies.GetCompanyById;

public class GetCompanyByIdQueryHandler : IQueryHandler<GetCompanyByIdQuery, CompanyResponse>
{
    private readonly IRepository<Company> _repository;
    public GetCompanyByIdQueryHandler(IRepository<Company> repository)
    {
        _repository = repository;
    }

    public async Task<CompanyResponse> Handle(GetCompanyByIdQuery query, CancellationToken cancellation)
    {
        var company = await _repository.GetByIdAsync(query.Id);

        if (company == null)
            throw new NotFoundException($"Company with id {query.Id} not found.");

        return new CompanyResponse
        {
            Id = company.Id,
            Name = company.Name,
            Website = company.Website,
            CreateDate = company.CreateDate
        };
    }
}