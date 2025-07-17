using MyCustomMediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MyCustomMediator.Deleagate;
namespace MyCustomMediator.Classes
{
    public class Sender : ISender
    {
        private readonly IServiceProvider _serviceProvider;

        public Sender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task<TResponse> SendToMediatoR<TRequest, TResponse>(TRequest request, CancellationToken token)
            where TRequest : IRequest<TResponse>
            where TResponse : class
        {
            Type handlerType = typeof(IRequestHandler<,>)
                .MakeGenericType(request.GetType(), typeof(TResponse)); //Get the handler type for the request
            
            dynamic handler = _serviceProvider.GetRequiredService(handlerType); //Get required service from the service provider

            var pipelines = _serviceProvider.GetServices<IPipeline<TRequest, TResponse>>(); //Get all pipeline requests

            if(pipelines == null || !pipelines.Any()) //Check if there are no pipelines
            {
                return handler.Handle(request, token); // Return the response directly from the handler if no pipelines are present
            }
            RequestHandlerDelegate<TResponse> handlerDelegate = () => handler.Handle(request, token); //Get the handle method from the handler

            foreach (var pipe in pipelines) // Iterate through each pipeline request
            {
                var next = handlerDelegate;
                handlerDelegate = () => pipe.SendToPipeline(request, next, token); // Create a new delegate that calls the pipeline request's SendToPipeline method
            }

            return handlerDelegate(); // Execute the final delegate to get the response
        }
    }
}
