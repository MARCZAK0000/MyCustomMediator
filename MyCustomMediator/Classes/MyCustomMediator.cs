using Microsoft.Extensions.DependencyInjection;
using MyCustomMediator.Interfaces;
using System.Reflection;

namespace MyCustomMediator.Classes
{
    public static class MyCustomMediator
    {
        public static void AddMyCustomMediator(this IServiceCollection services, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetExecutingAssembly(); // Default to the current assembly

            services.AddScoped<ISender, Sender>(); //Register the ISender implementation

            var interfaceType = typeof(IRequestHandler<,>); // Get the IRequestHandler interface type

            var handlerTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface)
                .SelectMany(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
                    .Select(i => new { Interfaces = i, Handler = type }));

            foreach (var item in handlerTypes)
            {
                services.AddScoped(item.Interfaces, item.Handler);    
            }
            
        }
    }
}
