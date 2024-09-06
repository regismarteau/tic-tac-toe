using Domain.ValueObjects;

namespace Domain.Events;

public record CellMarked(Player Player, Cell Cell) : IDomainEvent;
