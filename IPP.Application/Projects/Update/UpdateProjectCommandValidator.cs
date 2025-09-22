using FluentValidation;

namespace IPP.Application.Projects.Update;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Project Id is required.");

        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("Company Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date cannot be before Start date.");
    }
}