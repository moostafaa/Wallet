using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Application.Commands.WalletAccount;
using WalletService.Domain.Aggregates.WalletAccountAggregate;
using WalletService.Domain.Interfaces;
using WalletService.Domain.ValueObjects;

namespace WalletService.Application.Handlers.WalletAccount
{
    public class CreateWalletAccountCommandHandler : IRequestHandler<CreateWalletAccountCommand, Guid>
    {
        private readonly IWalletAccountRepository _accountRepository;
        private readonly IWalletRepository _walletRepository;
        
        public CreateWalletAccountCommandHandler(
            IWalletAccountRepository accountRepository,
            IWalletRepository walletRepository)
        {
            _accountRepository = accountRepository;
            _walletRepository = walletRepository;
        }
        
        public async Task<Guid> Handle(CreateWalletAccountCommand request, CancellationToken cancellationToken)
        {
            // Create account
            var accountId = Guid.NewGuid();
            var account = new Domain.Aggregates.WalletAccountAggregate.WalletAccount(
                accountId,
                request.Name,
                request.Type,
                request.Email,
                request.PhoneNumber,
                request.PrimaryCurrency);
                
            await _accountRepository.AddAsync(account);
            
            // Create default wallet in primary currency
            var walletId = Guid.NewGuid();
            var currency = Currency.FromCode(request.PrimaryCurrency);
            var wallet = new Domain.Aggregates.WalletAggregate.Wallet(walletId, accountId, currency);
            
            await _walletRepository.AddAsync(wallet);
            
            // Add wallet to account
            account.AddWallet(walletId);
            await _accountRepository.UpdateAsync(account);
            
            return accountId;
        }
    }
}