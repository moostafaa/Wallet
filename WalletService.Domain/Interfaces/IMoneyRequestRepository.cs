using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface IMoneyRequestRepository : IRepository<MoneyRequest>
    {
        Task<MoneyRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<MoneyRequest>> GetPendingRequestsForUserAsync(Guid userId);
        Task<MoneyRequest> AddAsync(MoneyRequest request);
        Task UpdateAsync(MoneyRequest request);
    }
}