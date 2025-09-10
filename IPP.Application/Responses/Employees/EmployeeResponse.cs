namespace Application.Responses.Employees;

public class EmployeeResponse
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime HireDate { get; set; }
}
