namespace Application.Commands.Companies;

public class UpdateCompanyCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Website { get; set; }
}
