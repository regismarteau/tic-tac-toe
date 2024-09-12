using Domain.DomainEvents;
using Domain.ValueObjects;

namespace Domain.Services;

public static class UserVersusComputer
{
    public const Player User = Player.X;
    public const Player Computer = Player.O;

    public static bool PlayedByComputer(this CellMarked cellMarked)
    {
        return cellMarked.Player == Computer;
    }
}