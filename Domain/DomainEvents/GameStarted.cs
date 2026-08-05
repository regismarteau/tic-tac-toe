using RMediator.Abstractions;

namespace Domain.DomainEvents;

public record GameStarted(GameId Id) : IDomainEvent;
