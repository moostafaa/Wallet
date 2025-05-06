using System;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;
using WalletService.Domain.Interfaces;
using WalletService.Infrastructure.EventSourcing;

namespace WalletService.Infrastructure.Repositories
{
    public class TopUpRequestRepository : ITopUpRequestRepository
    {
        private readonly AggregateRepository<TopUpRequest> _repository;
        
        public TopUpRequestRepository(IEventStore eventStore)
        {
            _repository = new AggregateRepository<TopUpRequest>(eventStore);
        }
        
        public async Task<TopUpRequest> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        
        public async Task<TopUpRequest> AddAsync(TopUpRequest topUpRequest)
        {
            await _repository.SaveAsync(topUpRequest, -1);
            return topUpRequest;
        }
        
        public async Task UpdateAsync(TopUpRequest topUpRequest)
        {
            await _repository.SaveAsync(topUpRequest, 0);
        }
        
        public async Task DeleteAsync(TopUpRequest entity)
        {
            throw new NotImplementedException("Delete is not supported in event sourcing");
        }
    }
}