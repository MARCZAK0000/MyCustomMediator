using MyCustomMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace MyCustomMediator.Classes
{
    public class Sender : ISender
    {
        private readonly IServiceProvider _serviceProvider;

        public Sender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task<TResponse> SendToMediatoR<TResponse>(IRequest<TResponse> request, CancellationToken token) where TResponse : class
        {
            Type handlerType = typeof(IRequestHandler<,>)
                .MakeGenericType(request.GetType(), typeof(TResponse)); //Get the handler type for the request

            dynamic handler = _serviceProvider.GetRequiredService(handlerType); //Get required service from the service provider
            return handler.Handle(request, token); // return Handler
        }
    }
}
