namespace IPP.Application.Projects.Commands.Create;

public class CreateProjectCommand
{
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = default!;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
}