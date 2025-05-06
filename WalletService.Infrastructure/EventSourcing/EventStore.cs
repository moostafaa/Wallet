using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Client;
using Newtonsoft.Json;
using WalletService.Domain.Interfaces;

namespace WalletService.Infrastructure.EventSourcing
{
    public class EventStore : IEventStore
    {
        private readonly EventStoreClient _eventStoreClient;
        private readonly ISnapshotStore _snapshotStore;
        private const int SnapshotThreshold = 50;
        
        public EventStore(EventStoreClient eventStoreClient, ISnapshotStore snapshotStore)
        {
            _eventStoreClient = eventStoreClient ?? throw new ArgumentNullException(nameof(eventStoreClient));
            _snapshotStore = snapshotStore ?? throw new ArgumentNullException(nameof(snapshotStore));
        }
        
        public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<object> events, int expectedVersion)
        {
            if (!events.Any())
                return;
                
            var streamName = GetStreamName(aggregateId);
            var eventData = events.Select(MapToEventData).ToArray();
            
            await _eventStoreClient.AppendToStreamAsync(
                streamName,
                expectedVersion == -1 ? StreamRevision.None : StreamRevision.FromInt64(expectedVersion),
                eventData);
                
            // Check if we need to create a snapshot
            var result = _eventStoreClient.ReadStreamAsync(
                Direction.Forwards,
                streamName,
                StreamPosition.Start);
                
            var eventCount = await result.CountAsync();
            
            if (eventCount >= SnapshotThreshold)
            {
                var aggregate = await GetAggregateFromEventsAsync(aggregateId, events.First().GetType().DeclaringType);
                if (aggregate != null)
                {
                    await _snapshotStore.SaveSnapshotAsync(aggregateId, aggregate, eventCount);
                }
            }
        }
        
        public async Task<IEnumerable<object>> GetEventsAsync(Guid aggregateId)
        {
            // Try to get the snapshot first
            var snapshot = await _snapshotStore.GetSnapshotAsync(aggregateId);
            var fromVersion = snapshot.HasValue ? snapshot.Value.Version : 0;
            
            var streamName = GetStreamName(aggregateId);
            var result = _eventStoreClient.ReadStreamAsync(
                Direction.Forwards,
                streamName,
                StreamPosition.FromInt64(fromVersion));
                
            if (await result.ReadState == ReadState.StreamNotFound)
                return Enumerable.Empty<object>();
                
            var events = new List<object>();
            
            if (snapshot.HasValue)
            {
                events.Add(snapshot.Value.Snapshot);
            }
            
            await foreach (var @event in result)
            {
                var eventData = @event.Event.Data.ToArray();
                var eventType = Encoding.UTF8.GetString(@event.Event.Metadata.ToArray());
                var eventJson = Encoding.UTF8.GetString(eventData);
                
                var type = Type.GetType(eventType);
                if (type == null)
                    continue;
                    
                var domainEvent = JsonConvert.DeserializeObject(eventJson, type);
                events.Add(domainEvent);
            }
            
            return events;
        }
        
        private EventData MapToEventData(object @event)
        {
            var eventType = @event.GetType();
            var eventData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
            var eventMetadata = Encoding.UTF8.GetBytes(eventType.AssemblyQualifiedName);
            
            return new EventData(
                Uuid.NewUuid(),
                eventType.Name,
                eventData,
                eventMetadata);
        }
        
        private string GetStreamName(Guid aggregateId)
        {
            return $"wallet-{aggregateId}";
        }
        
        private async Task<object> GetAggregateFromEventsAsync(Guid aggregateId, Type aggregateType)
        {
            var events = await GetEventsAsync(aggregateId);
            if (!events.Any())
                return null;
                
            var aggregate = Activator.CreateInstance(aggregateType, true);
            
            foreach (var @event in events)
            {
                var applyMethod = aggregateType.GetMethod("ApplyEvent", 
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public,
                    null,
                    new[] { @event.GetType() },
                    null);
                    
                if (applyMethod != null)
                {
                    applyMethod.Invoke(aggregate, new[] { @event });
                }
            }
            
            return aggregate;
        }
    }
}