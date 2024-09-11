namespace Database.Entities;

public class GameEntity
{
    public Guid Id { get; set; }
    public ResultValue Result { get; set; }
    public IReadOnlyCollection<MarkEntity> Marks { get; set; } = default!;
}

public enum ResultValue
{
    Undetermined = 0,
    WonByPlayerX = 1,
    WonByPlayerO = 2,
    Draw = 3,
}
