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
    public class CreateAutoTopUpRuleCommandHandler : IRequestHandler<CreateAutoTopUpRuleCommand, Guid>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IAutoTopUpRuleRepository _autoTopUpRuleRepository;
        
        public CreateAutoTopUpRuleCommandHandler(
            IWalletRepository walletRepository,
            IAutoTopUpRuleRepository autoTopUpRuleRepository)
        {
            _walletRepository = walletRepository;
            _autoTopUpRuleRepository = autoTopUpRuleRepository;
        }
        
        public async Task<Guid> Handle(CreateAutoTopUpRuleCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
            if (wallet == null)
                throw new InvalidOperationException($"Wallet {request.WalletId} not found");
                
            var rule = new AutoTopUpRule(
                Guid.NewGuid(),
                request.WalletId,
                new Money(request.TriggerAmount, wallet.Currency),
                new Money(request.TopUpAmount, wallet.Currency),
                request.Source);
                
            await _autoTopUpRuleRepository.AddAsync(rule);
            
            return rule.Id;
        }
    }
}