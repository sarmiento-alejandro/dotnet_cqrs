using CQRS_Lib.BaseInterfaces;

namespace CQRS_Lib.Dispatchers;

public interface ICommandDispatcher
{
    Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : ICommand<TResult>;
}