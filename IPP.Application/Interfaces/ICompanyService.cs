using Application.Commands.Companies;
using Application.Queries.Companies;
using Application.Responses.Companies;
using Application.Responses.Common;

namespace Application.Interfaces;

public interface ICompanyService
{
    Task<PagedResponse<CompanyResponse>> GetCompaniesAsync(GetCompaniesQuery query);
    Task<CompanyResponse?> GetCompanyByIdAsync(GetCompanyByIdQuery query);
    Task<CompanyResponse> CreateCompanyAsync(CreateCompanyCommand command);
    Task<CompanyResponse?> UpdateCompanyAsync(UpdateCompanyCommand command);
    Task<bool> DeleteCompanyAsync(DeleteCompanyCommand command);
}