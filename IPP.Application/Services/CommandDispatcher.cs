using FluentValidation;
using FluentValidation.Results;
using IPP.Application.Projects.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace IPP.Application.Services;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation)
    {
        var validators = _serviceProvider.GetServices<IValidator<TCommand>>();
        var validationFailures = new List<ValidationFailure>();

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(command, cancellation);
            if (!result.IsValid)
                validationFailures.AddRange((IEnumerable<ValidationFailure>)result.Errors);
        }

        if (validationFailures.Any())
        {
            var errorMessages = string.Join(" ", validationFailures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}"));
            throw new ValidationException(errorMessages);
        }

        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();
        return await handler.Handle(command, cancellation);
    }
}