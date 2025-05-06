using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Application.Commands.Wallet;
using WalletService.Domain.Interfaces;
using WalletService.Domain.ValueObjects;

namespace WalletService.Application.Handlers.Wallet
{
    public class AddFundsCommandHandler : IRequestHandler<AddFundsCommand, bool>
    {
        private readonly IWalletRepository _walletRepository;
        
        public AddFundsCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
        }
        
        public async Task<bool> Handle(AddFundsCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
            if (wallet == null)
                throw new InvalidOperationException($"Wallet {request.WalletId} not found");
                
            var money = new Money(request.Amount, wallet.Currency);
            wallet.AddFunds(money, request.Source, request.Reference);
            
            await _walletRepository.UpdateAsync(wallet);
            
            return true;
        }
    }
}