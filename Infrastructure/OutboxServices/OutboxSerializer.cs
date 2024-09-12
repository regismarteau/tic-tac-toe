using Database.Entities;
using Domain.DomainEvents;
using Newtonsoft.Json;

namespace Infrastructure.OutboxServices;

public static class OutboxSerializer
{
    public static OutboxEventEntity Serialize(this Event @event)
    {
        return new OutboxEventEntity
        {
            EventId = Guid.NewGuid(),
            Json = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            })
        };
    }

    public static Event Deserialize(this OutboxEventEntity entity)
    {
        return JsonConvert.DeserializeObject<Event>(entity.Json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        }) ?? throw new InvalidOperationException("Unable to deserialize event");
    }
}
