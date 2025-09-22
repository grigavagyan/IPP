namespace Domain.Entities;

public class Project
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public required string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ICollection<EmployeeProject> EmployeeProjects { get; } = new List<EmployeeProject>();
}