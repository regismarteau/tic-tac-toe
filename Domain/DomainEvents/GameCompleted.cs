using Domain.Gameplay;

namespace Domain.DomainEvents;

public record GameCompleted : Event;

public record GameWon(GameId Id, Player By) : GameCompleted;

public record GameResultedAsADraw(GameId Id) : GameCompleted;

