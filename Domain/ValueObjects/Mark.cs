namespace Domain.ValueObjects;

public record Mark(Player Player, Cell Cell)
{
    public bool PlayedByX => this.Player == Player.X;
    public bool PlayedByO => this.Player == Player.O;
}
