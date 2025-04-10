using CQRS_Lib.BaseInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS_Lib.Dispatchers;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : ICommand<TResult>
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return handler.HandleAsync(command, cancellationToken);
    }
}