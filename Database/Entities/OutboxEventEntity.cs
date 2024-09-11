namespace Database.Entities;

public class OutboxEventEntity
{
    public Guid EventId { get; set; }
    public string Json { get; set; } = default!;
}
