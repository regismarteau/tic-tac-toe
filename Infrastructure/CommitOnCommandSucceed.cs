using Database;
using MediatR;
using UseCases.Commands;

namespace Infrastructure;

public class CommitOnCommandSucceed<TCommand, TResponse>(TicTacToeDbContext dbContext) : IPipelineBehavior<TCommand, TResponse>
    where TCommand : notnull
{
    public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is ICommand or ICommand<TResponse>)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        return response;
    }
}