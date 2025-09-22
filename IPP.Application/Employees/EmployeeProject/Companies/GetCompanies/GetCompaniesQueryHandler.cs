using Application.Responses.Common;
using Application.Responses.Companies;
using Domain.Entities;
using Domain.Enums;
using IPP.Application.Projects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Employees.EmployeeProject.Companies.GetCompanies
{
    public class GetCompaniesQueryHandler : IQueryHandler<GetCompaniesQuery, PagedResponse<CompanyResponse>>
    {
        private readonly IRepository<Company> _repository;

        public GetCompaniesQueryHandler(IRepository<Company> repository)
        {
            _repository = repository;
        }
        public async Task<PagedResponse<CompanyResponse>> Handle(GetCompaniesQuery query, CancellationToken cancellation)
        {
            var companyQuery = _repository.Query().AsNoTracking();

            companyQuery = query.CompaniesSorting switch
            {
                CompaniesSorting.Name => query.SortOrder == SortOrder.Asc
                    ? companyQuery.OrderBy(x => x.Name)
                    : companyQuery.OrderByDescending(x => x.Name),

                CompaniesSorting.Website => query.SortOrder == SortOrder.Asc
                    ? companyQuery.OrderBy(x => x.Website)
                    : companyQuery.OrderByDescending(x => x.Website),

                CompaniesSorting.CreateDate => query.SortOrder == SortOrder.Asc
                    ? companyQuery.OrderBy(x => x.CreateDate)
                    : companyQuery.OrderByDescending(x => x.CreateDate),

                _ => query.SortOrder == SortOrder.Asc
                    ? companyQuery.OrderBy(x => x.Id)
                    : companyQuery.OrderByDescending(x => x.Id),
            };

            if (!string.IsNullOrEmpty(query.SearchText))
            {
                var parts = query.SearchText
                    .Split([" ", ", "], StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.ToLower());

                foreach (var part in parts)
                    companyQuery = companyQuery.Where(c => c.Name.ToLower().Contains(part));
            }

            if (query.CreatedDateFrom.HasValue)
                companyQuery = companyQuery.Where(c => c.CreateDate >= query.CreatedDateFrom.Value);

            if (query.CreatedDateTo.HasValue)
                companyQuery = companyQuery.Where(c => c.CreateDate <= query.CreatedDateTo.Value);

            var total = await companyQuery.CountAsync(cancellation);
            var items = await companyQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(c => new CompanyResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Website = c.Website,
                    CreateDate = c.CreateDate
                })
                .ToListAsync(cancellation);

            return new PagedResponse<CompanyResponse>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = total
            };
        }
    }
}