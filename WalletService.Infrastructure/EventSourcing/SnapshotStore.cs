using System;
using System.Threading.Tasks;
using EventStore.Client;
using Newtonsoft.Json;
using System.Text;
using WalletService.Domain.Interfaces;

namespace WalletService.Infrastructure.EventSourcing
{
    public class SnapshotStore : ISnapshotStore
    {
        private readonly EventStoreClient _eventStoreClient;
        
        public SnapshotStore(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient ?? throw new ArgumentNullException(nameof(eventStoreClient));
        }
        
        public async Task<(object Snapshot, int Version)?> GetSnapshotAsync(Guid aggregateId)
        {
            var streamName = GetSnapshotStreamName(aggregateId);
            
            try
            {
                var result = _eventStoreClient.ReadStreamAsync(
                    Direction.Backwards,
                    streamName,
                    StreamPosition.End,
                    1);
                    
                if (await result.ReadState == ReadState.StreamNotFound)
                    return null;
                    
                var eventData = await result.FirstAsync();
                var snapshotData = eventData.Event.Data.ToArray();
                var snapshotType = Encoding.UTF8.GetString(eventData.Event.Metadata.ToArray());
                var snapshotJson = Encoding.UTF8.GetString(snapshotData);
                
                var type = Type.GetType(snapshotType);
                if (type == null)
                    return null;
                    
                var snapshot = JsonConvert.DeserializeObject(snapshotJson, type);
                var version = (int)eventData.Event.Position.CommitPosition;
                
                return (snapshot, version);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public async Task SaveSnapshotAsync(Guid aggregateId, object snapshot, int version)
        {
            var streamName = GetSnapshotStreamName(aggregateId);
            var snapshotType = snapshot.GetType();
            var snapshotData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(snapshot));
            var snapshotMetadata = Encoding.UTF8.GetBytes(snapshotType.AssemblyQualifiedName);
            
            var eventData = new EventData(
                Uuid.NewUuid(),
                "Snapshot",
                snapshotData,
                snapshotMetadata);
                
            await _eventStoreClient.AppendToStreamAsync(
                streamName,
                StreamState.Any,
                new[] { eventData });
        }
        
        private string GetSnapshotStreamName(Guid aggregateId)
        {
            return $"snapshot-wallet-{aggregateId}";
        }
    }
}