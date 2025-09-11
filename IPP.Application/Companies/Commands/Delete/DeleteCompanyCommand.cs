namespace IPP.Application.Companies.Commands.Delete;

public class DeleteCompanyCommand
{
    public Guid Id { get; set; }
    public bool Force { get; set; } = false;
}
