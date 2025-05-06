using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface IDisputeRepository : IRepository<DisputeRequest>
    {
        Task<DisputeRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<DisputeRequest>> GetPendingDisputesAsync();
        Task<DisputeRequest> AddAsync(DisputeRequest dispute);
        Task UpdateAsync(DisputeRequest dispute);
    }
}