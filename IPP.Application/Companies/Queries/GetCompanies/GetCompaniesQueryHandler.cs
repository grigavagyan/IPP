using Application.Responses.Common;
using Application.Responses.Companies;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Companies.Queries.GetCompanies
{
    public class GetCompaniesQueryHandler : IQueryHandler<GetCompaniesQuery, PagedResponse<CompanyResponse>>
    {
        private readonly DataContext _dataContext;
        public GetCompaniesQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<PagedResponse<CompanyResponse>> Handle(GetCompaniesQuery query, CancellationToken cancellation)
        {
            var companies = _dataContext.Companies.AsQueryable();

            // ToDo Change SortBy and SortOrder to work with enums
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                companies = query.SortOrder.ToLower() == "desc"
                    ? companies.OrderByDescending(c => EF.Property<object>(c, query.SortBy))
                    : companies.OrderBy(c => EF.Property<object>(c, query.SortBy));
            }

            // ToDo implement SearchText, CreatedDateFrom and CreatedDateTo

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
    }
}