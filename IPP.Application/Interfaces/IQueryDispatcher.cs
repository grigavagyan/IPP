namespace IPP.Application.Interfaces;

public interface IQueryDispatcher
{
    Task<TQueryResult> Dispatch<TQuery, TQueryResult>(
        TQuery query,
        CancellationToken cancellation = default);
}