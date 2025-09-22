using FluentValidation;

namespace IPP.Application.Employees.Update;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Employee Id is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(200).WithMessage("Email cannot exceed 200 characters.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company Id is required.");
    }
}