
using Database;
using Database.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Queries;

public record GetGameState(Guid Id) : IQuery<GameDto>;

public class GetGameStateQueryHandler(TicTacToeDbContext dbContext) : QueryHandler<GetGameState, GameDto>
{
    protected override async Task<GameDto> Handle(GetGameState command)
    {
        return await dbContext.Games
            .ById(command.Id)
            .Select(game => new GameDto(
                (ResultDto)game.Result,
                game.Marks.Select(mark => new MarkDto((SymbolDto)mark.Player, (CellDto)mark.Cell)).ToList()))
            .SingleAsync();
    }
}

public record GameDto(ResultDto Result, IReadOnlyCollection<MarkDto> Marks);

public record MarkDto(SymbolDto Symbol, CellDto Cell);

public enum SymbolDto
{
    Cross = 0,
    Nought = 1,
}

public enum CellDto
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

public enum ResultDto
{
    Undetermined = 0,
    WonByPlayerX = 1,
    WonByPlayerO = 2,
    Draw = 3,
}