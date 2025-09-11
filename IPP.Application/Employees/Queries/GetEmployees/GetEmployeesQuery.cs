using Application.Common;

namespace IPP.Application.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : ListQueryParams
{
    public Guid? CompanyId { get; set; }
    public string EmailContains { get; set; }
    public DateTime? HireDateFrom { get; set; }
    public DateTime? HireDateTo { get; set; }
}