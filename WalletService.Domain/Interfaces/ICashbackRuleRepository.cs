using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface ICashbackRuleRepository : IRepository<CashbackRule>
    {
        Task<CashbackRule> GetByIdAsync(Guid id);
        Task<IEnumerable<CashbackRule>> GetActiveRulesAsync();
        Task<CashbackRule> AddAsync(CashbackRule rule);
        Task UpdateAsync(CashbackRule rule);
    }
}