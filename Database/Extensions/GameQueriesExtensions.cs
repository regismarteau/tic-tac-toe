using Database.Entities;

namespace Database.Extensions;

public static class GameQueriesExtensions
{
    public static IQueryable<GameEntity> ById(this IQueryable<GameEntity> query, Guid id)
    {
        return query.Where(entity => entity.Id == id);
    }
}
