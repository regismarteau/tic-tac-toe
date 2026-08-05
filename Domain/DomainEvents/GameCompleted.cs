using Domain.Gameplay;
using RMediator.Abstractions;

namespace Domain.DomainEvents;

public record GameCompleted : IDomainEvent;

public record GameWon(GameId Id, Player By) : GameCompleted;

public record GameResultedAsADraw(GameId Id) : GameCompleted;

