using Application.Responses.Employees;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;

namespace IPP.Application.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQueryHandler : IQueryHandler<GetEmployeeByIdQuery, EmployeeResponse>
{
    private readonly DataContext _dataContext;

    public GetEmployeeByIdQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<EmployeeResponse> Handle(GetEmployeeByIdQuery query, CancellationToken cancellation)
    {
        var employee = await _dataContext.Employees.FindAsync(query.Id);
        return employee == null ? null : new EmployeeResponse
        {
            Id = employee.Id,
            CompanyId = employee.CompanyId,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            HireDate = employee.HireDate
        };
    }
}
