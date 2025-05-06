using System;
using System.Threading.Tasks;

namespace WalletService.Domain.Interfaces
{
    public interface ISnapshotStore
    {
        Task<(object Snapshot, int Version)?> GetSnapshotAsync(Guid aggregateId);
        Task SaveSnapshotAsync(Guid aggregateId, object snapshot, int version);
    }
}