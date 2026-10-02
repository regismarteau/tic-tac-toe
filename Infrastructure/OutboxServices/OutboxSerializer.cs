using Database.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RMediator.Abstractions;

namespace Infrastructure.OutboxServices;

public static class OutboxSerializer
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        SerializationBinder = new DomainEventsBinder()
    };

    public static OutboxEventEntity Serialize(this IDomainEvent @event) => new()
    {
        EventId = Guid.NewGuid(),
        Json = JsonConvert.SerializeObject(@event, typeof(IDomainEvent), Settings)
    };

    public static IDomainEvent Deserialize(this OutboxEventEntity entity) => JsonConvert.DeserializeObject<IDomainEvent>(entity.Json, Settings)
        ?? throw new InvalidOperationException("Unable to deserialize event");

    private sealed class DomainEventsBinder : DefaultSerializationBinder
    {
        public override Type BindToType(string? assemblyName, string typeName)
        {
            var type = base.BindToType(assemblyName, typeName);
            return typeof(IDomainEvent).IsAssignableFrom(type) ? type : throw new JsonSerializationException($"Type {typeName} is not a domain event");
        }
    }
}
