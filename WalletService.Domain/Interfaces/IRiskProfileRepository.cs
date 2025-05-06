using System;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAccountAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface IRiskProfileRepository : IRepository<RiskProfile>
    {
        Task<RiskProfile> GetByIdAsync(Guid id);
        Task<RiskProfile> GetByAccountIdAsync(Guid accountId);
        Task<RiskProfile> AddAsync(RiskProfile profile);
        Task UpdateAsync(RiskProfile profile);
    }
}