using FluentValidation;
using IPP.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IPP.Application.Services;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public async Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation)
    {
        var validators = serviceProvider.GetServices<IValidator<TCommand>>();
        var validationFailures = new List<ValidationFailure>();

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(command, cancellation);
            if (!result.IsValid)
                validationFailures.AddRange((IEnumerable<ValidationFailure>)result.Errors);
        }

        if (validationFailures.Count > 0)
            throw new FluentValidation.ValidationException((IEnumerable<FluentValidation.Results.ValidationFailure>)validationFailures);

        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();
        return await handler.Handle(command, cancellation);
    }
}