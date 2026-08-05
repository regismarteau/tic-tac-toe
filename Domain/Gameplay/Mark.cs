using Domain.Gameplay;

namespace Domain.ValueObjects;

public record Mark(Player Player, Cell Cell)
{
    public bool PlayedByX => Player == Player.X;
    public bool PlayedByO => Player == Player.O;
}
