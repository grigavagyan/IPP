namespace Application.Responses.Companies;

public class CompanyResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Website { get; set; }
    public DateTime CreateDate { get; set; }
}