namespace Database;

public class MarkEntity
{
    public Guid GameId { get; set; }

    public CellValue Cell { get; set; }

    public PlayerValue Player { get; set; }
}

public enum CellValue
{
    TopLeft = 0,
    TopMiddle = 1,
    TopRight = 2,
    Left = 3,
    Middle = 4,
    Right = 5,
    BottomLeft = 6,
    BottomMiddle = 7,
    BottomRight = 8
}

public enum PlayerValue
{
    X = 0,
    O = 1
}