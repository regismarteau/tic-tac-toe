using Database;
using RMediator.Abstractions;

namespace Infrastructure;

public class CommitOnCommandSucceed<TCommand, TResponse>(TicTacToeDbContext dbContext) : IHandleMiddleware<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TCommand request, NextMiddleware<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next(request, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return response;
    }
}

public class CommitOnCommandSucceed<TCommand>(TicTacToeDbContext dbContext) : IHandleMiddleware<TCommand>
    where TCommand : ICommand
{
    public async Task Handle(TCommand request, NextMiddleware next, CancellationToken cancellationToken)
    {
        try
        {
            await next(request, cancellationToken);

        }
        catch (Exception ex)
        {

            throw;
        }
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}