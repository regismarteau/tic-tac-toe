using Database;
using Database.Entities;
using Database.Extensions;
using Domain;
using Domain.DomainEvents;
using Domain.ValueObjects;
using Infrastructure.OutboxServices;
using Microsoft.EntityFrameworkCore;
using UseCases.Ports;

namespace Infrastructure;

public class GameRepository(TicTacToeDbContext dbContext) : IFindGame, IStoreGame
{
    public async Task<Game> Get(GameId id)
    {
        var marks = await dbContext.Marks
            .Where(mark => mark.GameId == id.Value)
            .Select(mark => new Mark(
                    mark.Player == PlayerValue.X ? Player.X : Player.O,
                    mark.Cell.Map()))
            .ToListAsync();

        return Game.Rehydrate(id, marks);
    }

    public async Task Store(Events events)
    {
        foreach (var @event in events)
        {
            await this.Handle(@event);
            await dbContext.Outbox.AddAsync(@event.Serialize());
        }
    }

    private Task Handle(Event @event)
    {
        return @event switch
        {
            GameStarted started => this.Handle(started),
            CellMarked marked => this.Handle(marked),
            GameWon won => this.Handle(won),
            GameResultedAsADraw draw => this.Handle(draw),
            _ => Task.CompletedTask,
        };
    }

    private async Task Handle(GameStarted started)
    {
        await dbContext.Games.AddAsync(new GameEntity
        {
            Id = started.Id.Value,
            Result = ResultValue.Undetermined
        });
    }

    private async Task Handle(GameWon won)
    {
        var game = await this.GetEntity(won.Id);
        game.Result = won.By == Player.X ? ResultValue.WonByPlayerX : ResultValue.WonByPlayerO;
    }

    private async Task Handle(GameResultedAsADraw draw)
    {
        var game = await this.GetEntity(draw.Id);
        game.Result = ResultValue.Draw;
    }

    private async Task Handle(CellMarked marked)
    {
        await dbContext.AddAsync(new MarkEntity
        {
            GameId = marked.GameId.Value,
            Player = marked.Player == Player.X ? PlayerValue.X : PlayerValue.O,
            Cell = marked.Cell.Map()
        });
    }

    private async Task<GameEntity> GetEntity(GameId id)
    {
        return await dbContext.Games.ById(id.Value).SingleAsync();
    }
}