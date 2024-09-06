namespace Domain.Events;

public record GameWon(Player By) : IDomainEvent;
