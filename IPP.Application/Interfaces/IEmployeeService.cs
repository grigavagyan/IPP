using Application.Commands.Employees;
using Application.Queries.Employees;
using Application.Responses.Employees;
using Application.Responses.Common;

namespace Application.Interfaces;

public interface IEmployeeService
{
    Task<PagedResponse<EmployeeResponse>> GetEmployeesAsync(GetEmployeesQuery query);
    Task<EmployeeResponse?> GetEmployeeByIdAsync(GetEmployeeByIdQuery query);
    Task<EmployeeResponse> CreateEmployeeAsync(CreateEmployeeCommand command);
    Task<EmployeeResponse?> UpdateEmployeeAsync(UpdateEmployeeCommand command);
    Task<bool> DeleteEmployeeAsync(DeleteEmployeeCommand command);
}
