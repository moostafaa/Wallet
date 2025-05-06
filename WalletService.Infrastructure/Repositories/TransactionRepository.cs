using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletService.Domain.Aggregates.TransactionAggregate;
using WalletService.Domain.Interfaces;
using WalletService.Infrastructure.EventSourcing;
using WalletService.Infrastructure.Persistence;

namespace WalletService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AggregateRepository<Transaction> _repository;
        private readonly ReadDbContext _readDbContext;
        
        public TransactionRepository(IEventStore eventStore, ReadDbContext readDbContext)
        {
            _repository = new AggregateRepository<Transaction>(eventStore);
            _readDbContext = readDbContext;
        }
        
        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId)
        {
            var transactions = await _readDbContext.Transactions
                .Where(t => t.SourceWalletId == walletId || t.DestinationWalletId == walletId)
                .ToListAsync();
                
            var result = new List<Transaction>();
            foreach (var transaction in transactions)
            {
                var aggregate = await GetByIdAsync(transaction.Id);
                if (aggregate != null)
                {
                    result.Add(aggregate);
                }
            }
            
            return result;
        }
        
        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            await _repository.SaveAsync(transaction, -1);
            return transaction;
        }
        
        public async Task UpdateAsync(Transaction transaction)
        {
            await _repository.SaveAsync(transaction, 0);
        }
        
        public async Task DeleteAsync(Transaction entity)
        {
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}