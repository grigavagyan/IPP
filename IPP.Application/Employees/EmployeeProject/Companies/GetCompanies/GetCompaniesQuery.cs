using Domain.Enums;

namespace IPP.Application.Employees.EmployeeProject.Companies.GetCompanies;

public class GetCompaniesQuery
{
    public string SearchText { get; set; }
    public DateTime? CreatedDateFrom { get; set; }
    public DateTime? CreatedDateTo { get; set; }
    public SortOrder SortOrder { get; set; }
    public CompaniesSorting CompaniesSorting { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}