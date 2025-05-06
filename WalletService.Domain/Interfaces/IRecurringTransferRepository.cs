using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface IRecurringTransferRepository : IRepository<RecurringTransfer>
    {
        Task<RecurringTransfer> GetByIdAsync(Guid id);
        Task<IEnumerable<RecurringTransfer>> GetActiveTransfersForWalletAsync(Guid walletId);
        Task<IEnumerable<RecurringTransfer>> GetDueTransfersAsync(DateTime dueDate);
        Task<RecurringTransfer> AddAsync(RecurringTransfer transfer);
        Task UpdateAsync(RecurringTransfer transfer);
    }
}