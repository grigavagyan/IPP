using Application.Commands.Employees;
using Application.Interfaces;
using Application.Queries.Employees;
using Application.Responses.Common;
using Application.Responses.Employees;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Common;

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _db;

    public EmployeeService(AppDbContext db) => _db = db;

    public async Task<PagedResponse<EmployeeResponse>> GetEmployeesAsync(GetEmployeesQuery query)
    {
        var employees = _db.Employees.AsQueryable();

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

    public async Task<EmployeeResponse?> GetEmployeeByIdAsync(GetEmployeeByIdQuery query)
    {
        var e = await _db.Employees.FindAsync(query.Id);
        return e == null ? null : new EmployeeResponse
        {
            Id = e.Id,
            CompanyId = e.CompanyId,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            HireDate = e.HireDate
        };
    }

    public async Task<EmployeeResponse> CreateEmployeeAsync(CreateEmployeeCommand command)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            HireDate = command.HireDate
        };

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();

        return new EmployeeResponse
        {
            Id = employee.Id,
            CompanyId = employee.CompanyId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            HireDate = employee.HireDate
        };
    }

    public async Task<EmployeeResponse?> UpdateEmployeeAsync(UpdateEmployeeCommand command)
    {
        var employee = await _db.Employees.FindAsync(command.Id);
        if (employee == null) return null;

        employee.FirstName = command.FirstName;
        employee.LastName = command.LastName;
        employee.Email = command.Email;
        employee.HireDate = command.HireDate;
        employee.CompanyId = command.CompanyId;

        await _db.SaveChangesAsync();

        return new EmployeeResponse
        {
            Id = employee.Id,
            CompanyId = employee.CompanyId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            HireDate = employee.HireDate
        };
    }

    public async Task<bool> DeleteEmployeeAsync(DeleteEmployeeCommand command)
    {
        var employee = await _db.Employees.FindAsync(command.Id);
        if (employee == null) return false;

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
        return true;
    }
}
