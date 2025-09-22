using FluentValidation;

namespace IPP.Application.Employees.EmployeeProject.Companies.Update;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Company Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(200).WithMessage("Company name cannot exceed 200 characters.");

        RuleFor(x => x.Website)
            .NotEmpty().WithMessage("Website is required.")
            .MaximumLength(500).WithMessage("Website cannot exceed 500 characters.");
    }
}