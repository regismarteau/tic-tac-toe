using Domain.ValueObjects;

namespace Domain.DomainEvents;

public record GameCompleted : Event;

public record GameWon(Player By) : GameCompleted;

public record GameResultedAsADraw : GameCompleted;

