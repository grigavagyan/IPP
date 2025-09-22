using Application.Responses.Common;
using Application.Responses.Employees;
using Domain.Entities;
using Domain.Enums;
using IPP.Application.Projects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Employees.GetEmployees;

public class GetEmployeesQueryHandler : IQueryHandler<GetEmployeesQuery, PagedResponse<EmployeeResponse>>
{
    private readonly IRepository<Employee> _repository;

    public GetEmployeesQueryHandler(IRepository<Employee> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<EmployeeResponse>> Handle(GetEmployeesQuery query, CancellationToken cancellation)
    {
        var employeeQuery = _repository.Query().Where(x => x.CompanyId == query.CompanyId).AsNoTracking();

        employeeQuery = query.EmployeesSorting switch
        {
            EmployeesSorting.FirstName => query.SortOrder == SortOrder.Asc
                ? employeeQuery.OrderBy(x => x.FirstName)
                : employeeQuery.OrderByDescending(x => x.FirstName),

            EmployeesSorting.LastName => query.SortOrder == SortOrder.Asc
                ? employeeQuery.OrderBy(x => x.LastName)
                : employeeQuery.OrderByDescending(x => x.LastName),

            EmployeesSorting.Email => query.SortOrder == SortOrder.Asc
                ? employeeQuery.OrderBy(x => x.Email)
                : employeeQuery.OrderByDescending(x => x.Email),

            EmployeesSorting.HireDate => query.SortOrder == SortOrder.Asc
                ? employeeQuery.OrderBy(x => x.HireDate)
                : employeeQuery.OrderByDescending(x => x.HireDate),

            _ => query.SortOrder == SortOrder.Asc
                ? employeeQuery.OrderBy(x => x.Id)
                : employeeQuery.OrderByDescending(x => x.Id),
        };

        if (!string.IsNullOrEmpty(query.SearchText))
        {
            var parts = query.SearchText
                .Split([" ", ", "], StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower());

            foreach (var part in parts)
                employeeQuery = employeeQuery.Where(c => c.FirstName.ToLower().Contains(part));
        }

        if (query.HireDateFrom.HasValue)
        {
            employeeQuery = employeeQuery.Where(c => c.HireDate >= query.HireDateFrom.Value);
        }

        if (query.HireDateTo.HasValue)
        {
            employeeQuery = employeeQuery.Where(c => c.HireDate <= query.HireDateTo.Value);
        }

        var total = await employeeQuery.CountAsync(cancellation);
        var items = await employeeQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(e => new EmployeeResponse
        {
            Id = e.Id,
            CompanyId = e.CompanyId,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            HireDate = e.HireDate
        }).ToListAsync(cancellation);

        return new PagedResponse<EmployeeResponse>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }
}