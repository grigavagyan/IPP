namespace Application.Commands.Companies;

public class CreateCompanyCommand
{
    public string Name { get; set; } = default!;
    public string? Website { get; set; }
}
