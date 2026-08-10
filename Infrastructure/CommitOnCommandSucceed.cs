using RMediator.Abstractions;

namespace Infrastructure;

public class CommitOnCommandSucceed<TCommand, TResponse>(DbContextSaveChanges changes) : IHandleMiddleware<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TCommand request, NextMiddleware<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next(request, cancellationToken);
        await changes.SaveAsync(cancellationToken);
        return response;
    }
}

public class CommitOnCommandSucceed<TCommand>(DbContextSaveChanges changes) : IHandleMiddleware<TCommand>
    where TCommand : ICommand
{
    public async Task Handle(TCommand request, NextMiddleware next, CancellationToken cancellationToken)
    {
        await next(request, cancellationToken);
        await changes.SaveAsync(cancellationToken);
    }
}
