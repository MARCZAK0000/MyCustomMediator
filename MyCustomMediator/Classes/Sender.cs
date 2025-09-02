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

        public async Task<TResponse> SendToMediator<TResponse>(IRequest<TResponse> request, CancellationToken token)
            where TResponse : class
        {
            Type handlerType = typeof(IRequestHandler<,>)
                .MakeGenericType(request.GetType(), typeof(TResponse)); //Get the handler type for the request
            
            dynamic handler = _serviceProvider.GetRequiredService(handlerType); //Get required service from the service provider
            
            var pipelines = _serviceProvider.GetServices<IPipeline<IRequest<TResponse>, TResponse>>(); //Get all pipeline requests
            
            if(!pipelines.Any()) //Check if there are no pipelines
            {
                return await handler.Handle((dynamic)request, token); // Return the response directly from the handler if no pipelines are present
            }
            RequestHandlerDelegate<TResponse> handlerDelegate = async () => await handler.Handle((dynamic)request, token); //Get the handle method from the handler

            foreach (var pipe in pipelines) // Iterate through each pipeline request
            {
                var next = handlerDelegate;
                handlerDelegate = async () => await pipe.SendToPipeline(request, next, token); // Create a new delegate that calls the pipeline request's SendToPipeline method
            }

            return await handlerDelegate(); // Execute the final delegate to get the response
        }
    }
}
