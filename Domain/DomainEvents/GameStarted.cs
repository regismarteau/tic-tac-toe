using Domain.ValueObjects;

namespace Domain.DomainEvents;

public record GameStarted(GameId Id) : Event;
