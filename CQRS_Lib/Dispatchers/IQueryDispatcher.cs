using CQRS_Lib.BaseInterfaces;

namespace CQRS_Lib.Dispatchers;

public interface IQueryDispatcher
{
    Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) 
        where TQuery : IQuery<TResult>;
}