using Domain;
using UseCases.Ports;

namespace UseCases
{
    public record StartAGame : ICommand;

    public class StartAGameCommandHandler(IStoreGame game) : CommandHandler<StartAGame>
    {
        protected override async Task Handle(StartAGame command)
        {
            await game.Store(Game.Start());
        }
    }
}
