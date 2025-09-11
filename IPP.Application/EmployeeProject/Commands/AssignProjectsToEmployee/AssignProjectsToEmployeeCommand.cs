namespace IPP.Application.EmployeeProject.Commands.AssignProjectsToEmployee;

public class AssignProjectsToEmployeeCommand
{
    public Guid EmployeeId { get; set; }
    public List<Guid> ProjectIds { get; set; } = new();
}
