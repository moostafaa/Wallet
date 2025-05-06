using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface IReferralBonusRepository : IRepository<ReferralBonus>
    {
        Task<ReferralBonus> GetByIdAsync(Guid id);
        Task<IEnumerable<ReferralBonus>> GetPendingBonusesAsync();
        Task<ReferralBonus> AddAsync(ReferralBonus bonus);
        Task UpdateAsync(ReferralBonus bonus);
    }
}