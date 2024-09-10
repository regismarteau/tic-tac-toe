using Domain.ValueObjects;

namespace Domain.DomainEvents;

public record GameCompleted : IEvent;

public record GameWon(Player By) : GameCompleted;

public record GameResultedAsADraw : GameCompleted;

