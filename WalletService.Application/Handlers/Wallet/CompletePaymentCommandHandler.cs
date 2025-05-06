using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WalletService.Application.Commands.Wallet;
using WalletService.Domain.Interfaces;

namespace WalletService.Application.Handlers.Wallet
{
    public class CompletePaymentCommandHandler : IRequestHandler<CompletePaymentCommand, bool>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPaymentRequestRepository _paymentRequestRepository;
        
        public CompletePaymentCommandHandler(
            IWalletRepository walletRepository,
            IPaymentRequestRepository paymentRequestRepository)
        {
            _walletRepository = walletRepository;
            _paymentRequestRepository = paymentRequestRepository;
        }
        
        public async Task<bool> Handle(CompletePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRequestRepository.GetByIdAsync(request.PaymentId);
            if (payment == null)
                throw new InvalidOperationException($"Payment {request.PaymentId} not found");
                
            var buyerWallet = await _walletRepository.GetByIdAsync(payment.BuyerWalletId);
            if (buyerWallet == null)
                throw new InvalidOperationException($"Buyer wallet {payment.BuyerWalletId} not found");
                
            var merchantWallet = await _walletRepository.GetByIdAsync(payment.MerchantWalletId);
            if (merchantWallet == null)
                throw new InvalidOperationException($"Merchant wallet {payment.MerchantWalletId} not found");
                
            // Transfer funds from buyer to merchant
            buyerWallet.TransferFunds(payment.Amount, payment.MerchantWalletId, payment.OrderReference);
            
            // Get transaction ID from the event
            var transferEvent = (Domain.Events.FundsTransferredEvent)buyerWallet.DomainEvents
                .FirstOrDefault(e => e is Domain.Events.FundsTransferredEvent);
                
            if (transferEvent == null)
                throw new InvalidOperationException("Transfer event not found");
                
            // Receive funds in merchant wallet
            merchantWallet.ReceiveFunds(
                payment.Amount,
                payment.BuyerWalletId,
                payment.OrderReference,
                transferEvent.TransactionId);
                
            // Complete payment request
            payment.Complete(transferEvent.TransactionId);
            
            // Save all changes
            await _walletRepository.UpdateAsync(buyerWallet);
            await _walletRepository.UpdateAsync(merchantWallet);
            await _paymentRequestRepository.UpdateAsync(payment);
            
            return true;
        }
    }
}