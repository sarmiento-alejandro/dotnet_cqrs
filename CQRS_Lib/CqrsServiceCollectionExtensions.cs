using System.Reflection;
using CQRS_Lib.BaseInterfaces;
using CQRS_Lib.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS_Lib;

public static class CqrsServiceCollectionExtensions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services, Assembly[] assemblies)
    {
        // Registrar los dispatchers
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        
        // Registrar automáticamente todos los handlers
        RegisterHandlers(services, assemblies);
        
        return services;
    }
    
    private static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        // Registrar command handlers
        var commandHandlerType = typeof(ICommandHandler<,>);
        RegisterHandlersOfType(services, assemblies, commandHandlerType);
        
        // Registrar query handlers
        var queryHandlerType = typeof(IQueryHandler<,>);
        RegisterHandlersOfType(services, assemblies, queryHandlerType);
    }
    
    private static void RegisterHandlersOfType(IServiceCollection services, Assembly[] assemblies, Type handlerType)
    {
        var handlers = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType) && !t.IsAbstract && !t.IsInterface);
            
        foreach (var handler in handlers)
        {
            var interfaceType = handler.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType);
                
            services.AddScoped(interfaceType, handler);
        }
    }
}