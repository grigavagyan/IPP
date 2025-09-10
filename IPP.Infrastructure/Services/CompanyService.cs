using Application.Commands.Companies;
using Application.Common;
using Application.Interfaces;
using Application.Queries.Companies;
using Application.Responses.Common;
using Application.Responses.Companies;
using Domain.Entities;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IPP.Infrastructure.Services;

public class CompanyService : ICompanyService
{
    private readonly AppDbContext _db;

    public CompanyService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResponse<CompanyResponse>> GetCompaniesAsync(GetCompaniesQuery query)
    {
        var companies = _db.Companies.AsQueryable();

        // Filtering
        if (!string.IsNullOrEmpty(query.FilterBy) && !string.IsNullOrEmpty(query.FilterValue))
        {
            if (query.FilterBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                companies = companies.Where(c => c.Name.Contains(query.FilterValue));
        }

        // Sorting
        if (!string.IsNullOrEmpty(query.SortBy))
        {
            companies = query.SortOrder.ToLower() == "desc"
                ? companies.OrderByDescending(c => EF.Property<object>(c, query.SortBy))
                : companies.OrderBy(c => EF.Property<object>(c, query.SortBy));
        }

        var total = await companies.CountAsync();
        var items = await companies
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(c => new CompanyResponse
            {
                Id = c.Id,
                Name = c.Name,
                Website = c.Website,
                CreateDate = c.CreateDate
            })
            .ToListAsync();

        return new PagedResponse<CompanyResponse>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    public async Task<CompanyResponse?> GetCompanyByIdAsync(GetCompanyByIdQuery query)
    {
        var c = await _db.Companies.FindAsync(query.Id);
        return c == null ? null : new CompanyResponse
        {
            Id = c.Id,
            Name = c.Name,
            Website = c.Website,
            CreateDate = c.CreateDate
        };
    }

    public async Task<CompanyResponse> CreateCompanyAsync(CreateCompanyCommand command)
    {
        var company = new Company
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Website = command.Website
        };

        _db.Companies.Add(company);
        await _db.SaveChangesAsync();

        return new CompanyResponse
        {
            Id = company.Id,
            Name = company.Name,
            Website = company.Website,
            CreateDate = company.CreateDate
        };
    }

    public async Task<CompanyResponse?> UpdateCompanyAsync(UpdateCompanyCommand command)
    {
        var company = await _db.Companies.FindAsync(command.Id);
        if (company == null) return null;

        company.Name = command.Name;
        company.Website = command.Website;

        await _db.SaveChangesAsync();

        return new CompanyResponse
        {
            Id = company.Id,
            Name = company.Name,
            Website = company.Website,
            CreateDate = company.CreateDate
        };
    }

    public async Task<bool> DeleteCompanyAsync(DeleteCompanyCommand command)
    {
        var company = await _db.Companies
            .Include(c => c.Employees)
            .Include(c => c.Projects)
            .FirstOrDefaultAsync(c => c.Id == command.Id);

        if (company == null) return false;

        if (!command.Force && (company.Employees.Any() || company.Projects.Any()))
        {
            throw new InvalidOperationException("Company has employees or projects. Use force=true to delete.");
        }

        _db.Companies.Remove(company);
        await _db.SaveChangesAsync();
        return true;
    }
}
