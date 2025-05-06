using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.TransactionAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Task<Transaction> GetByIdAsync(Guid id);
        Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId);
        Task<Transaction> AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
    }
}