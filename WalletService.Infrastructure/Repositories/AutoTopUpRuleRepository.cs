using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;
using WalletService.Domain.Interfaces;
using WalletService.Infrastructure.EventSourcing;
using WalletService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace WalletService.Infrastructure.Repositories
{
    public class AutoTopUpRuleRepository : IAutoTopUpRuleRepository
    {
        private readonly AggregateRepository<AutoTopUpRule> _repository;
        private readonly ReadDbContext _readDbContext;
        
        public AutoTopUpRuleRepository(IEventStore eventStore, ReadDbContext readDbContext)
        {
            _repository = new AggregateRepository<AutoTopUpRule>(eventStore);
            _readDbContext = readDbContext;
        }
        
        public async Task<AutoTopUpRule> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<AutoTopUpRule>> GetByWalletIdAsync(Guid walletId)
        {
            var rules = await _readDbContext.AutoTopUpRules
                .Where(r => r.WalletId == walletId && r.IsActive)
                .ToListAsync();
                
            var result = new List<AutoTopUpRule>();
            foreach (var rule in rules)
            {
                var aggregate = await GetByIdAsync(rule.Id);
                if (aggregate != null)
                {
                    result.Add(aggregate);
                }
            }
            
            return result;
        }
        
        public async Task<AutoTopUpRule> AddAsync(AutoTopUpRule rule)
        {
            await _repository.SaveAsync(rule, -1);
            return rule;
        }
        
        public async Task UpdateAsync(AutoTopUpRule rule)
        {
            await _repository.SaveAsync(rule, 0);
        }
        
        public async Task DeleteAsync(AutoTopUpRule entity)
        {
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}