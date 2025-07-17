namespace MyCustomMediator.Interfaces
{
    public interface ISender
    {
        /// <summary>
        /// Sends a request to the Mediator and asynchronously retrieves the response.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response expected from the Mediator. Must be a reference type.</typeparam>
        /// <param name="request">The request to be sent to the Mediator. Cannot be null.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response from the Mediator.</returns>
        Task<TResponse> SendToMediatoR<TResponse>(IRequest<TResponse> request, 
            CancellationToken token) where TResponse : class;
    }
}
