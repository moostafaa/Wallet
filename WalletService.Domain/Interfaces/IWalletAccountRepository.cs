using System;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAccountAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface IWalletAccountRepository : IRepository<WalletAccount>
    {
        Task<WalletAccount> GetByIdAsync(Guid id);
        Task<WalletAccount> AddAsync(WalletAccount account);
        Task UpdateAsync(WalletAccount account);
    }
}