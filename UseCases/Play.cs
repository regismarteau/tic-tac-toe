﻿using Domain.ValueObjects;
using UseCases.Ports;

namespace UseCases;

public record Play(GameId Id, Cell Cell) : ICommand;

public class PlayCommandHandler(IFindGame finder, IStoreGame store) : CommandHandler<Play>
{
    protected override async Task Handle(Play command)
    {
        var game = await finder.Get(command.Id);
        var events = game.Play(Player.X, command.Cell);
        await store.Store(events);
    }
}
