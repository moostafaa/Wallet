```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface ISettlementBatchRepository : IRepository<SettlementBatch>
    {
        Task<SettlementBatch> GetByIdAsync(Guid id);
        Task<IEnumerable<SettlementBatch>> GetPendingBatchesForMerchantAsync(Guid merchantId);
        Task<SettlementBatch> AddAsync(SettlementBatch batch);
        Task UpdateAsync(SettlementBatch batch);
    }
}
```