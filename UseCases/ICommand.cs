using MediatR;

namespace UseCases
{
    public interface ICommand : IRequest;

    public abstract class CommandHandler<TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
    {
        protected abstract Task Handle(TCommand command);
        Task IRequestHandler<TCommand>.Handle(TCommand request, CancellationToken cancellationToken)
        {
            return this.Handle(request);
        }
    }
}
