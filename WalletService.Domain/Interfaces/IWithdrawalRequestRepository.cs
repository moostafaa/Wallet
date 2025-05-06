```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface IWithdrawalRequestRepository : IRepository<WithdrawalRequest>
    {
        Task<WithdrawalRequest> GetByIdAsync(Guid id);
        Task<IEnumerable<WithdrawalRequest>> GetPendingRequestsAsync();
        Task<WithdrawalRequest> AddAsync(WithdrawalRequest request);
        Task UpdateAsync(WithdrawalRequest request);
    }
}
```