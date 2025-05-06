using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Application.Commands.Wallet;
using WalletService.Domain.Aggregates.WalletAggregate;
using WalletService.Domain.Interfaces;
using WalletService.Domain.ValueObjects;

namespace WalletService.Application.Handlers.Wallet
{
    public class InitiateTopUpCommandHandler : IRequestHandler<InitiateTopUpCommand, Guid>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITopUpRequestRepository _topUpRequestRepository;
        
        public InitiateTopUpCommandHandler(
            IWalletRepository walletRepository,
            ITopUpRequestRepository topUpRequestRepository)
        {
            _walletRepository = walletRepository;
            _topUpRequestRepository = topUpRequestRepository;
        }
        
        public async Task<Guid> Handle(InitiateTopUpCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
            if (wallet == null)
                throw new InvalidOperationException($"Wallet {request.WalletId} not found");
                
            var topUpRequest = new TopUpRequest(
                Guid.NewGuid(),
                request.WalletId,
                new Money(request.Amount, wallet.Currency),
                request.Source,
                request.ExternalReference);
                
            await _topUpRequestRepository.AddAsync(topUpRequest);
            
            return topUpRequest.Id;
        }
    }
}