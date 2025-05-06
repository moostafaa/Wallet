using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Application.Commands.Wallet;
using WalletService.Domain.Interfaces;
using WalletService.Domain.ValueObjects;

namespace WalletService.Application.Handlers.Wallet
{
    public class TransferFundsCommandHandler : IRequestHandler<TransferFundsCommand, bool>
    {
        private readonly IWalletRepository _walletRepository;
        
        public TransferFundsCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
        }
        
        public async Task<bool> Handle(TransferFundsCommand request, CancellationToken cancellationToken)
        {
            var sourceWallet = await _walletRepository.GetByIdAsync(request.SourceWalletId);
            if (sourceWallet == null)
                throw new InvalidOperationException($"Source wallet {request.SourceWalletId} not found");
                
            var destinationWallet = await _walletRepository.GetByIdAsync(request.DestinationWalletId);
            if (destinationWallet == null)
                throw new InvalidOperationException($"Destination wallet {request.DestinationWalletId} not found");
                
            // Check if currencies match
            if (sourceWallet.Currency != destinationWallet.Currency)
                throw new InvalidOperationException("Cannot transfer between wallets with different currencies");
                
            var money = new Money(request.Amount, sourceWallet.Currency);
            
            // Perform transfer
            sourceWallet.TransferFunds(money, request.DestinationWalletId, request.Reference);
            
            // Get transaction ID from the event
            var transferEvent = (Domain.Events.FundsTransferredEvent)sourceWallet.DomainEvents
                .FirstOrDefault(e => e is Domain.Events.FundsTransferredEvent);
                
            if (transferEvent == null)
                throw new InvalidOperationException("Transfer event not found");
                
            // Receive funds in destination wallet
            destinationWallet.ReceiveFunds(
                money,
                request.SourceWalletId,
                request.Reference,
                transferEvent.TransactionId);
                
            // Save both wallets
            await _walletRepository.UpdateAsync(sourceWallet);
            await _walletRepository.UpdateAsync(destinationWallet);
            
            return true;
        }
    }
}