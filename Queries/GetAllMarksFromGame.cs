
using Database;
using Microsoft.EntityFrameworkCore;

namespace Queries;

public record GetAllMarksFromGame(Guid Id) : IQuery<MarksDto>;

public class GetAllMarksFromQueryHandler(TicTacToeDbContext dbContext) : QueryHandler<GetAllMarksFromGame, MarksDto>
{
    protected override async Task<MarksDto> Handle(GetAllMarksFromGame command)
    {
        return new(await dbContext.Marks
            .Select(mark => new MarkDto((SymbolDto)mark.Player, (CellDto)mark.Cell))
            .ToListAsync());
    }
}

public record MarksDto(IReadOnlyCollection<MarkDto> Marks);

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