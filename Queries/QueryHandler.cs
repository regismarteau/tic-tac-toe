using MediatR;

namespace Queries;

public interface IQuery<TOut> : IRequest<TOut>;

public abstract class QueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    protected abstract Task<TResponse> Handle(TQuery command);
    Task<TResponse> IRequestHandler<TQuery, TResponse>.Handle(TQuery request, CancellationToken cancellationToken)
    {
        return this.Handle(request);
    }
}