using System;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAccountAggregate;
using WalletService.Domain.Interfaces;
using WalletService.Infrastructure.EventSourcing;

namespace WalletService.Infrastructure.Repositories
{
    public class WalletAccountRepository : IWalletAccountRepository
    {
        private readonly AggregateRepository<WalletAccount> _repository;
        
        public WalletAccountRepository(IEventStore eventStore)
        {
            _repository = new AggregateRepository<WalletAccount>(eventStore);
        }
        
        public async Task<WalletAccount> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<WalletAccount> AddAsync(WalletAccount account)
        {
            await _repository.SaveAsync(account, -1);
            return account;
        }
        
        public async Task UpdateAsync(WalletAccount account)
        {
            await _repository.SaveAsync(account, 0);
        }
        
        public async Task DeleteAsync(WalletAccount entity)
        {
            // Not implemented for event sourcing
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}