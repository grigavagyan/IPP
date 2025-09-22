using Domain.Enums;

namespace IPP.Application.Employees.GetEmployees;

public class GetEmployeesQuery
{
    public Guid? CompanyId { get; set; }
    public string SearchText { get; set; }
    public DateTime? HireDateFrom { get; set; }
    public DateTime? HireDateTo { get; set; }
    public SortOrder SortOrder { get; set; }
    public EmployeesSorting EmployeesSorting { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}