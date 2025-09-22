using Domain.Enums;

namespace IPP.Application.Projects.GetProjects;

public class GetProjectsQuery
{
    public Guid? CompanyId { get; set; }
    public string SearchText { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public SortOrder SortOrder { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}