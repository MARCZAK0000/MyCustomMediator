namespace MyCustomMediator.Interfaces
{
    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        /// <summary>
        /// Handles the specified request and returns a response asynchronously.
        /// </summary>
        /// <param name="request">The request object containing the data to process. Cannot be <see langword="null"/>.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response object.</returns>
        Task<TResponse> Handle(TRequest request, CancellationToken token);
    }
}
