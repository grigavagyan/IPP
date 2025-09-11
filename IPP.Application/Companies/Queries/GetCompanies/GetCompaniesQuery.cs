namespace IPP.Application.Companies.Queries.GetCompanies;

public class GetCompaniesQuery
{
    public string SortBy { get; set; }
    public string SortOrder { get; set; } = "asc";
    public string SearchText { get; set; }
    public DateTime? CreatedDateFrom { get; set; }
    public DateTime? CreatedDateTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
