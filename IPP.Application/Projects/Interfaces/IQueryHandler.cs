namespace IPP.Application.Projects.Interfaces;

public interface IQueryHandler<in TQuery, TQueryResult>
{
    Task<TQueryResult> Handle(TQuery query, CancellationToken cancellation);
}