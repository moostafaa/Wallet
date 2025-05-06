using System;
using System.Threading.Tasks;
using WalletService.Domain.Aggregates.WalletAggregate;

namespace WalletService.Domain.Interfaces
{
    public interface ITopUpRequestRepository : IRepository<TopUpRequest>
    {
        Task<TopUpRequest> GetByIdAsync(Guid id);
        Task<TopUpRequest> AddAsync(TopUpRequest topUpRequest);
        Task UpdateAsync(TopUpRequest topUpRequest);
    }
}