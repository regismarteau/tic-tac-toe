using Domain;
using Domain.DomainEvents;
using RMediator.Abstractions;
using UseCases.Ports;

namespace UseCases.Commands;

public record StartAGame : ICommand<Guid>;

public class StartAGameCommandHandler(IStoreGame store) : IHandleCommand<StartAGame, Guid>
{
    public async Task<Guid> Handle(StartAGame command, CancellationToken cancellationToken)
    {
        var gameStarted = Game.Start();
        await store.Store(Events.Raise(gameStarted));
        return gameStarted.Id.Value;
    }
}
