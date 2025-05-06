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
    public class MoneyRequestRepository : IMoneyRequestRepository
    {
        private readonly AggregateRepository<MoneyRequest> _repository;
        private readonly ReadDbContext _readDbContext;
        
        public MoneyRequestRepository(IEventStore eventStore, ReadDbContext readDbContext)
        {
            _repository = new AggregateRepository<MoneyRequest>(eventStore);
            _readDbContext = readDbContext;
        }
        
        public async Task<MoneyRequest> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<MoneyRequest>> GetPendingRequestsForUserAsync(Guid userId)
        {
            var requests = await _readDbContext.MoneyRequests
                .Where(r => r.RequesteeId == userId && r.Status == "Pending")
                .ToListAsync();
                
            var result = new List<MoneyRequest>();
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
        
        public async Task<MoneyRequest> AddAsync(MoneyRequest request)
        {
            await _repository.SaveAsync(request, -1);
            return request;
        }
        
        public async Task UpdateAsync(MoneyRequest request)
        {
            await _repository.SaveAsync(request, 0);
        }
        
        public async Task DeleteAsync(MoneyRequest entity)
        {
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}