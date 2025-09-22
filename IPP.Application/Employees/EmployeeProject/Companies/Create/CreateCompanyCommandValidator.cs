using FluentValidation;

namespace IPP.Application.Employees.EmployeeProject.Companies.Create;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(200).WithMessage("Company name cannot exceed 200 characters.");

        RuleFor(x => x.Website)
            .NotEmpty().WithMessage("Website is required.")
            .MaximumLength(500).WithMessage("Website cannot exceed 500 characters.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Website must be a valid URL.");
    }
}