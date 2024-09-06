namespace Domain.Events;

public record GameCompleted : IDomainEvent;

public record GameWon(Player By) : GameCompleted;

public record Draw : GameCompleted;

