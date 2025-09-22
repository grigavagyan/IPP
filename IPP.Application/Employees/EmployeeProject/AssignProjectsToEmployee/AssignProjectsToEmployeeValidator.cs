using FluentValidation;

namespace IPP.Application.Employees.EmployeeProject.AssignProjectsToEmployee;

public class AssignProjectsToEmployeeCommandValidator : AbstractValidator<AssignProjectsToEmployeeCommand>
{
    public AssignProjectsToEmployeeCommandValidator()
    {
        RuleFor(x => x.ProjectIds)
            .NotNull().WithMessage("Project Ids cannot be null.")
            .Must(p => p.Distinct().Count() == p.Count)
            .WithMessage("Project Ids cannot contain duplicates.");
    }
}