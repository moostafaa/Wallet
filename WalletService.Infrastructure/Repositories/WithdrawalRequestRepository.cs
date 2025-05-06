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
    public class WithdrawalRequestRepository : IWithdrawalRequestRepository
    {
        private readonly AggregateRepository<WithdrawalRequest> _repository;
        private readonly ReadDbContext _readDbContext;
        
        public WithdrawalRequestRepository(IEventStore eventStore, ReadDbContext readDbContext)
        {
            _repository = new AggregateRepository<WithdrawalRequest>(eventStore);
            _readDbContext = readDbContext;
        }
        
        public async Task<WithdrawalRequest> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<WithdrawalRequest>> GetPendingRequestsAsync()
        {
            var requests = await _readDbContext.WithdrawalRequests
                .Where(r => r.Status == "Pending")
                .ToListAsync();
                
            var result = new List<WithdrawalRequest>();
            foreach (var request in requests)
            {
                var aggregate = await GetByIdAsync(request.Id);
                if (aggregate != null)
                {
                    result.Add(aggregate);
                }
            }
            
            return result;
        }
        
        public async Task<WithdrawalRequest> AddAsync(WithdrawalRequest request)
        {
            await _repository.SaveAsync(request, -1);
            return request;
        }
        
        public async Task UpdateAsync(WithdrawalRequest request)
        {
            await _repository.SaveAsync(request, 0);
        }
        
        public async Task DeleteAsync(WithdrawalRequest entity)
        {
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}
```