namespace IPP.Application.Companies.Commands.Update;

public class UpdateCompanyCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Website { get; set; }
}
