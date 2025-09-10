namespace Application.Commands.Companies;

public class DeleteCompanyCommand
{
    public Guid Id { get; set; }
    public bool Force { get; set; } = false;
}
