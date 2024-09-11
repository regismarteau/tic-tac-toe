using Database;
using Domain;
using Domain.DomainEvents;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
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
            await this.Persist(@event);
        }
    }

    private async Task Persist(Event @event)
    {
        await dbContext.Outbox.AddAsync(new OutboxEventEntity
        {
            EventId = Guid.NewGuid(),
            Json = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            })
        });
    }

    private Task Handle(Event @event)
    {
        return @event switch
        {
            GameStarted started => this.Handle(started),
            CellMarked marked => this.Handle(marked),
        };
    }

    private async Task Handle(GameStarted started)
    {
        await Task.CompletedTask;
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
}

public static class CellMapper
{
    public static Cell Map(this CellValue cell)
    {
        return cell switch
        {
            CellValue.TopLeft => Cell.TopLeft,
            CellValue.TopMiddle => Cell.TopMiddle,
            CellValue.TopRight => Cell.TopRight,
            CellValue.Left => Cell.Left,
            CellValue.Middle => Cell.Middle,
            CellValue.Right => Cell.Right,
            CellValue.BottomLeft => Cell.BottomLeft,
            CellValue.BottomMiddle => Cell.BottomMiddle,
            CellValue.BottomRight => Cell.BottomRight
        };
    }
    public static CellValue Map(this Cell cell)
    {
        return cell switch
        {
            Cell.TopLeft => CellValue.TopLeft,
            Cell.TopMiddle => CellValue.TopMiddle,
            Cell.TopRight => CellValue.TopRight,
            Cell.Left => CellValue.Left,
            Cell.Middle => CellValue.Middle,
            Cell.Right => CellValue.Right,
            Cell.BottomLeft => CellValue.BottomLeft,
            Cell.BottomMiddle => CellValue.BottomMiddle,
            Cell.BottomRight => CellValue.BottomRight
        };
    }
}
