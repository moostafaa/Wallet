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
    public class RecurringTransferRepository : IRecurringTransferRepository
    {
        private readonly AggregateRepository<RecurringTransfer> _repository;
        private readonly ReadDbContext _readDbContext;
        
        public RecurringTransferRepository(IEventStore eventStore, ReadDbContext readDbContext)
        {
            _repository = new AggregateRepository<RecurringTransfer>(eventStore);
            _readDbContext = readDbContext;
        }
        
        public async Task<RecurringTransfer> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<RecurringTransfer>> GetActiveTransfersForWalletAsync(Guid walletId)
        {
            var transfers = await _readDbContext.RecurringTransfers
                .Where(t => t.SourceWalletId == walletId && t.IsActive)
                .ToListAsync();
                
            var result = new List<RecurringTransfer>();
            foreach (var transfer in transfers)
            {
                var aggregate = await GetByIdAsync(transfer.Id);
                if (aggregate != null)
                {
                    result.Add(aggregate);
                }
            }
            
            return result;
        }
        
        public async Task<IEnumerable<RecurringTransfer>> GetDueTransfersAsync(DateTime dueDate)
        {
            var transfers = await _readDbContext.RecurringTransfers
                .Where(t => t.IsActive && t.NextExecutionDate <= dueDate)
                .ToListAsync();
                
            var result = new List<RecurringTransfer>();
            foreach (var transfer in transfers)
            {
                var aggregate = await GetByIdAsync(transfer.Id);
                if (aggregate != null)
                {
                    result.Add(aggregate);
                }
            }
            
            return result;
        }
        
        public async Task<RecurringTransfer> AddAsync(RecurringTransfer transfer)
        {
            await _repository.SaveAsync(transfer, -1);
            return transfer;
        }
        
        public async Task UpdateAsync(RecurringTransfer transfer)
        {
            await _repository.SaveAsync(transfer, 0);
        }
        
        public async Task DeleteAsync(RecurringTransfer entity)
        {
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}