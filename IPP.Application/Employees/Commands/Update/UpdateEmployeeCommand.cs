namespace IPP.Application.Employees.Commands.Update;

public class UpdateEmployeeCommand
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime HireDate { get; set; } = DateTime.UtcNow;
}