using Domain.Gameplay;
using RMediator.Abstractions;

namespace Domain.DomainEvents;

public record CellMarked(GameId GameId, Player Player, Cell Cell) : IDomainEvent;
