using Application.Common;

namespace IPP.Application.Projects.Queries.GetProjects;

public class GetProjectsQuery : ListQueryParams
{
    public Guid? CompanyId { get; set; }
    public string NameContains { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public bool ActiveOnly { get; set; } = false;
}
