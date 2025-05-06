using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WalletService.Domain.Interfaces
{
    public interface IEventStore
    {
        Task SaveEventsAsync(Guid aggregateId, IEnumerable<object> events, int expectedVersion);
        Task<IEnumerable<object>> GetEventsAsync(Guid aggregateId);
    }
}