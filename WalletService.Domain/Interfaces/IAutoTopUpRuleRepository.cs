using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface IAutoTopUpRuleRepository : IRepository<AutoTopUpRule>
    {
        Task<AutoTopUpRule> GetByIdAsync(Guid id);
        Task<IEnumerable<AutoTopUpRule>> GetByWalletIdAsync(Guid walletId);
        Task<AutoTopUpRule> AddAsync(AutoTopUpRule rule);
        Task UpdateAsync(AutoTopUpRule rule);
    }
}