using Application.Common;
using Application.Responses.Common;
using Application.Responses.Employees;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler : IQueryHandler<GetEmployeesQuery, PagedResponse<EmployeeResponse>>
{
    private readonly DataContext _dataContext;

    public GetEmployeesQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<PagedResponse<EmployeeResponse>> Handle(GetEmployeesQuery query, CancellationToken cancellation)
    {
        var employees = _dataContext.Employees.AsQueryable();

        // ToDo Change
        if (query.CompanyId.HasValue)
            employees = employees.Where(e => e.CompanyId == query.CompanyId);

        if (!string.IsNullOrEmpty(query.EmailContains))
            employees = employees.Where(e => e.Email.Contains(query.EmailContains));

        if (query.HireDateFrom.HasValue)
            employees = employees.Where(e => e.HireDate >= query.HireDateFrom.Value);

        if (query.HireDateTo.HasValue)
            employees = employees.Where(e => e.HireDate <= query.HireDateTo.Value);

        var total = await employees.CountAsync();

        employees = employees
            .ApplySorting(query.SortBy, query.SortOrder)
            .ApplyPagination(query.Page, query.PageSize);

        var items = await employees.Select(e => new EmployeeResponse
        {
            Id = e.Id,
            CompanyId = e.CompanyId,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            HireDate = e.HireDate
        }).ToListAsync();

        return new PagedResponse<EmployeeResponse>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }
}
