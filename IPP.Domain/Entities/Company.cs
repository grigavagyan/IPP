namespace Domain.Entities;

public class Company
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Website { get; set; }
    public DateTime CreateDate { get; init; } = DateTime.UtcNow;

    public ICollection<Employee> Employees { get; } = [];
    public ICollection<Project> Projects { get; } = [];
}