using System;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<Wallet> GetByIdAsync(Guid id);
        Task<Wallet> AddAsync(Wallet wallet);
        Task UpdateAsync(Wallet wallet);
    }
}