```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletService.Domain.Aggregates.WalletAggregate;
using WalletService.Domain.Interfaces;
using WalletService.Infrastructure.EventSourcing;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.Repositories
{
    public class SettlementBatchRepository : ISettlementBatchRepository
    {
        private readonly AggregateRepository<SettlementBatch> _repository;
        private readonly ReadDbContext _readDbContext;
        
        public SettlementBatchRepository(IEventStore eventStore, ReadDbContext readDbContext)
        {
            _repository = new AggregateRepository<SettlementBatch>(eventStore);
            _readDbContext = readDbContext;
        }
        
        public async Task<SettlementBatch> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<SettlementBatch>> GetPendingBatchesForMerchantAsync(Guid merchantId)
        {
            var batches = await _readDbContext.SettlementBatches
                .Where(b => b.MerchantId == merchantId && b.Status == "Pending")
                .ToListAsync();
                
            var result = new List<SettlementBatch>();
            foreach (var batch in batches)
            {
                var aggregate = await GetByIdAsync(batch.Id);
                if (aggregate != null)
                {
                    result.Add(aggregate);
                }
            }
            
            return result;
        }
        
        public async Task<SettlementBatch> AddAsync(SettlementBatch batch)
        {
            await _repository.SaveAsync(batch, -1);
            return batch;
        }
        
        public async Task UpdateAsync(SettlementBatch batch)
        {
            await _repository.SaveAsync(batch, 0);
        }
        
        public async Task DeleteAsync(SettlementBatch entity)
        {
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}
```