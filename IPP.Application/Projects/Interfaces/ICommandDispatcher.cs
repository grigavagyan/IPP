﻿namespace IPP.Application.Projects.Interfaces;

public interface ICommandDispatcher
{
    Task<TCommandResult> Dispatch<TCommand, TCommandResult>(
        TCommand command,
        CancellationToken cancellation = default);
}