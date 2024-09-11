using Database.Entities;
using Domain.ValueObjects;

namespace Infrastructure;

public static class CellMapper
{
    public static Cell Map(this CellValue cell)
    {
        return cell switch
        {
            CellValue.TopLeft => Cell.TopLeft,
            CellValue.TopMiddle => Cell.TopMiddle,
            CellValue.TopRight => Cell.TopRight,
            CellValue.Left => Cell.Left,
            CellValue.Middle => Cell.Middle,
            CellValue.Right => Cell.Right,
            CellValue.BottomLeft => Cell.BottomLeft,
            CellValue.BottomMiddle => Cell.BottomMiddle,
            CellValue.BottomRight => Cell.BottomRight,
            _ => throw new NotImplementedException()
        };
    }
    public static CellValue Map(this Cell cell)
    {
        return cell switch
        {
            Cell.TopLeft => CellValue.TopLeft,
            Cell.TopMiddle => CellValue.TopMiddle,
            Cell.TopRight => CellValue.TopRight,
            Cell.Left => CellValue.Left,
            Cell.Middle => CellValue.Middle,
            Cell.Right => CellValue.Right,
            Cell.BottomLeft => CellValue.BottomLeft,
            Cell.BottomMiddle => CellValue.BottomMiddle,
            Cell.BottomRight => CellValue.BottomRight,
            _ => throw new NotImplementedException()
        };
    }
}
