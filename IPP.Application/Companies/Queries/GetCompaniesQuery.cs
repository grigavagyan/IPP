namespace Application.Queries.Companies;

public class GetCompaniesQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public string SortOrder { get; set; } = "asc";
    public string? FilterBy { get; set; }
    public string? FilterValue { get; set; }
}
