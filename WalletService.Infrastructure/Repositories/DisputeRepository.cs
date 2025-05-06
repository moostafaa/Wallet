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
    public class DisputeRepository : IDisputeRepository
    {
        private readonly AggregateRepository<DisputeRequest> _repository;
        private readonly ReadDbContext _readDbContext;
        
        public DisputeRepository(IEventStore eventStore, ReadDbContext readDbContext)
        {
            _repository = new AggregateRepository<DisputeRequest>(eventStore);
            _readDbContext = readDbContext;
        }
        
        public async Task<DisputeRequest> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<DisputeRequest>> GetPendingDisputesAsync()
        {
            var disputes = await _readDbContext.Disputes
                .Where(d => d.Status == "Pending")
                .ToListAsync();
                
            var result = new List<DisputeRequest>();
            foreach (var dispute in disputes)
            {
                var aggregate = await GetByIdAsync(dispute.Id);
                if (aggregate != null)
                {
                    result.Add(aggregate);
                }
            }
            
            return result;
        }
        
        public async Task<DisputeRequest> AddAsync(DisputeRequest dispute)
        {
            await _repository.SaveAsync(dispute, -1);
            return dispute;
        }
        
        public async Task UpdateAsync(DisputeRequest dispute)
        {
            await _repository.SaveAsync(dispute, 0);
        }
        
        public async Task DeleteAsync(DisputeRequest entity)
        {
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}