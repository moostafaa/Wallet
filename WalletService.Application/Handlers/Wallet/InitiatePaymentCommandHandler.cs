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
    public class InitiatePaymentCommandHandler : IRequestHandler<InitiatePaymentCommand, Guid>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPaymentRequestRepository _paymentRequestRepository;
        
        public InitiatePaymentCommandHandler(
            IWalletRepository walletRepository,
            IPaymentRequestRepository paymentRequestRepository)
        {
            _walletRepository = walletRepository;
            _paymentRequestRepository = paymentRequestRepository;
        }
        
        public async Task<Guid> Handle(InitiatePaymentCommand request, CancellationToken cancellationToken)
        {
            var buyerWallet = await _walletRepository.GetByIdAsync(request.BuyerWalletId);
            if (buyerWallet == null)
                throw new InvalidOperationException($"Buyer wallet {request.BuyerWalletId} not found");
                
            var merchantWallet = await _walletRepository.GetByIdAsync(request.MerchantWalletId);
            if (merchantWallet == null)
                throw new InvalidOperationException($"Merchant wallet {request.MerchantWalletId} not found");
                
            if (buyerWallet.Currency != merchantWallet.Currency)
                throw new InvalidOperationException("Buyer and merchant wallets must have the same currency");
                
            var paymentRequest = new PaymentRequest(
                Guid.NewGuid(),
                request.BuyerWalletId,
                request.MerchantWalletId,
                new Money(request.Amount, buyerWallet.Currency),
                request.OrderReference,
                request.Description);
                
            await _paymentRequestRepository.AddAsync(paymentRequest);
            
            return paymentRequest.Id;
        }
    }
}