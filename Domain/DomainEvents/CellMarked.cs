using Domain.ValueObjects;

namespace Domain.DomainEvents;

public record CellMarked(GameId GameId, Player Player, Cell Cell) : Event;