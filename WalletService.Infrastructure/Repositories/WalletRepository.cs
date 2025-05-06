using System;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;
using WalletService.Domain.Interfaces;
using WalletService.Infrastructure.EventSourcing;

namespace WalletService.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AggregateRepository<Wallet> _repository;
        
        public WalletRepository(IEventStore eventStore)
        {
            _repository = new AggregateRepository<Wallet>(eventStore);
        }
        
        public async Task<Wallet> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<Wallet> AddAsync(Wallet wallet)
        {
            await _repository.SaveAsync(wallet, -1);
            return wallet;
        }
        
        public async Task UpdateAsync(Wallet wallet)
        {
            await _repository.SaveAsync(wallet, 0);
        }
        
        public async Task DeleteAsync(Wallet entity)
        {
            // Not implemented for event sourcing
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}