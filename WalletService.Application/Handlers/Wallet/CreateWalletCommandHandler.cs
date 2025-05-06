using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Application.Commands.Wallet;
using WalletService.Domain.Aggregates.WalletAccountAggregate;
using WalletService.Domain.Aggregates.WalletAggregate;
using WalletService.Domain.Interfaces;
using WalletService.Domain.ValueObjects;

namespace WalletService.Application.Handlers.Wallet
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Guid>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletAccountRepository _accountRepository;
        
        public CreateWalletCommandHandler(
            IWalletRepository walletRepository,
            IWalletAccountRepository accountRepository)
        {
            _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }
        
        public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            // Validate account exists
            var account = await _accountRepository.GetByIdAsync(request.AccountId);
            if (account == null)
                throw new InvalidOperationException($"Account {request.AccountId} not found");
                
            if (account.Status != AccountStatus.Active)
                throw new InvalidOperationException($"Account {request.AccountId} is not active");
                
            // Create wallet
            var currency = Currency.FromCode(request.CurrencyCode);
            var walletId = Guid.NewGuid();
            var wallet = new Domain.Aggregates.WalletAggregate.Wallet(walletId, request.AccountId, currency);
            
            // Add wallet to account
            account.AddWallet(walletId);
            
            // Save both aggregate roots
            await _walletRepository.AddAsync(wallet);
            await _accountRepository.UpdateAsync(account);
            
            return walletId;
        }
    }
}