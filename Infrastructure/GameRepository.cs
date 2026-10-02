using Database;
using Database.Entities;
using Database.Exceptions;
using Database.Extensions;
using Domain;
using Domain.DomainEvents;
using Domain.Gameplay;
using Infrastructure.OutboxServices;
using Microsoft.EntityFrameworkCore;
using RMediator.Abstractions;
using UseCases.Ports;

namespace Infrastructure;

public class GameRepository(TicTacToeDbContext dbContext) : IFindGame, IStoreGame
{
    public async Task<Game> Get(GameId id, CancellationToken cancellationToken)
    {
        var game = await dbContext.Games
            .ById(id.Value)
            .Select(game => new
            {
                game.Id,
                Marks = game.Marks.Select(mark => new
                {
                    mark.Player,
                    mark.Cell
                }).ToList()
            })
            .SingleOrDefaultAsync(cancellationToken) ?? throw new GameNotFoundException();

        return Game.Rehydrate(new(game.Id), game.Marks.Select(mark => ToMark(mark.Player, mark.Cell)).ToList());
    }

    public async Task Store(Events events, CancellationToken cancellationToken)
    {
        foreach (var @event in events)
        {
            await Handle(@event, cancellationToken);
            await dbContext.Outbox.AddAsync(@event.Serialize(), cancellationToken);
        }
    }

    private static Mark ToMark(PlayerValue player, CellValue cell) => new(player == PlayerValue.X ? Player.X : Player.O, cell.Map());

    private Task Handle(IDomainEvent @event, CancellationToken cancellationToken) => @event switch
    {
        GameStarted started => Handle(started, cancellationToken),
        CellMarked marked => Handle(marked, cancellationToken),
        GameWon won => Handle(won, cancellationToken),
        GameResultedAsADraw draw => Handle(draw, cancellationToken),
        _ => Task.CompletedTask
    };

    private async Task Handle(GameStarted started, CancellationToken cancellationToken) => await dbContext.Games.AddAsync(new()
    {
        Id = started.Id.Value,
        Result = ResultValue.Undetermined
    }, cancellationToken);

    private async Task Handle(GameWon won, CancellationToken cancellationToken)
    {
        var game = await GetEntity(won.Id, cancellationToken);
        game.Result = won.By == Player.X ? ResultValue.WonByPlayerX : ResultValue.WonByPlayerO;
    }

    private async Task Handle(GameResultedAsADraw draw, CancellationToken cancellationToken)
    {
        var game = await GetEntity(draw.Id, cancellationToken);
        game.Result = ResultValue.Draw;
    }

    private async Task Handle(CellMarked marked, CancellationToken cancellationToken) => await dbContext.AddAsync(new MarkEntity
    {
        GameId = marked.GameId.Value,
        Player = marked.Player == Player.X ? PlayerValue.X : PlayerValue.O,
        Cell = marked.Cell.Map()
    }, cancellationToken);

    private async Task<GameEntity> GetEntity(GameId id, CancellationToken cancellationToken) => await dbContext.Games.ById(id.Value).SingleAsync(cancellationToken);
}
