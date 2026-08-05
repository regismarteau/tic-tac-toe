using Database.Entities;
using Newtonsoft.Json;
using RMediator.Abstractions;

namespace Infrastructure.OutboxServices;

public static class OutboxSerializer
{
    public static OutboxEventEntity Serialize(this IDomainEvent @event)
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

    public static IDomainEvent Deserialize(this OutboxEventEntity entity)
    {
        return JsonConvert.DeserializeObject<IDomainEvent>(entity.Json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        }) ?? throw new InvalidOperationException("Unable to deserialize event");
    }
}
