using MyCustomMediator.Deleagate;
using static MyCustomMediator.Classes.Sender;

namespace MyCustomMediator.Interfaces
{
    public interface IPipeline<TRequest, TResponse> where TResponse : class
         where TRequest : IRequest<TResponse>
    {
        Task<TResponse> SendToPipeline(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token);
    }
}
