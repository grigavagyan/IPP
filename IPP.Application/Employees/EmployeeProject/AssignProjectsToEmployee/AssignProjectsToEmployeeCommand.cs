namespace IPP.Application.Employees.EmployeeProject.AssignProjectsToEmployee;

public class AssignProjectsToEmployeeCommand
{
    public Guid EmployeeId { get; set; }
    public List<Guid> ProjectIds { get; set; } = new();
}