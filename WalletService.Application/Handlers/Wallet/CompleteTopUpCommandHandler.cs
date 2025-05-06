using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Application.Commands.Wallet;
using WalletService.Domain.Interfaces;

namespace WalletService.Application.Handlers.Wallet
{
    public class CompleteTopUpCommandHandler : IRequestHandler<CompleteTopUpCommand, bool>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITopUpRequestRepository _topUpRequestRepository;
        
        public CompleteTopUpCommandHandler(
            IWalletRepository walletRepository,
            ITopUpRequestRepository topUpRequestRepository)
        {
            _walletRepository = walletRepository;
            _topUpRequestRepository = topUpRequestRepository;
        }
        
        public async Task<bool> Handle(CompleteTopUpCommand request, CancellationToken cancellationToken)
        {
            var topUpRequest = await _topUpRequestRepository.GetByIdAsync(request.RequestId);
            if (topUpRequest == null)
                throw new InvalidOperationException($"Top-up request {request.RequestId} not found");
                
            var wallet = await _walletRepository.GetByIdAsync(topUpRequest.WalletId);
            if (wallet == null)
                throw new InvalidOperationException($"Wallet {topUpRequest.WalletId} not found");
                
            // Complete the top-up request
            topUpRequest.Complete(request.TransactionReference);
            
            // Add funds to wallet
            wallet.AddFunds(topUpRequest.Amount, topUpRequest.Source, request.TransactionReference);
            
            // Save changes
            await _topUpRequestRepository.UpdateAsync(topUpRequest);
            await _walletRepository.UpdateAsync(wallet);
            
            return true;
        }
    }
}