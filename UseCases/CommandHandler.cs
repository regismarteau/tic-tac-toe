using MediatR;

namespace UseCases;

public interface ICommand : IRequest;
public interface ICommand<TResponse> : IRequest<TResponse>;

public abstract class CommandHandler<TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
{
    protected abstract Task Handle(TCommand command);
    Task IRequestHandler<TCommand>.Handle(TCommand request, CancellationToken cancellationToken)
    {
        return this.Handle(request);
    }
}


public abstract class CommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    protected abstract Task<TResponse> Handle(TCommand command);
    Task<TResponse> IRequestHandler<TCommand, TResponse>.Handle(TCommand request, CancellationToken cancellationToken)
    {
        return this.Handle(request);
    }
}
