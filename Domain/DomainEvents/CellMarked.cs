using Domain.Gameplay;
using Domain.ValueObjects;
using RMediator.Abstractions;

namespace Domain.DomainEvents;

public record CellMarked(GameId GameId, Player Player, Cell Cell) : IDomainEvent;