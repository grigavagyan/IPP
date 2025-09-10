namespace Domain.Entities;

public class Employee
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public DateTime HireDate { get; set; }

    public ICollection<EmployeeProject> EmployeeProjects { get; } = new List<EmployeeProject>();
}
