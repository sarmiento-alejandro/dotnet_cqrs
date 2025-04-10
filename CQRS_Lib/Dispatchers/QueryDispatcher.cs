using CQRS_Lib.BaseInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS_Lib.Dispatchers;

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) 
        where TQuery : IQuery<TResult>
    {
        var handler = serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
        return handler.HandleAsync(query, cancellationToken);
    }
}