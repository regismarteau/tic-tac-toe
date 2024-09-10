using Database;
using MediatR;
using UseCases;

namespace Infrastructure;

public class CommitOnCommandSucceed<TCommand, TResponse>(TicTacToeDbContext dbContext) : IPipelineBehavior<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();
        await dbContext.SaveChangesAsync(cancellationToken);
        return response;
    }
}