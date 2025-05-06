using System;

namespace WalletService.Domain.Events
{
    public class PaymentRequestCreatedEvent : DomainEvent
    {
        public Guid PaymentId { get; }
        public Guid BuyerWalletId { get; }
        public Guid MerchantWalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string OrderReference { get; }
        public string Description { get; }
        
        public PaymentRequestCreatedEvent(
            Guid paymentId,
            Guid buyerWalletId,
            Guid merchantWalletId,
            decimal amount,
            string currencyCode,
            string orderReference,
            string description)
        {
            PaymentId = paymentId;
            BuyerWalletId = buyerWalletId;
            MerchantWalletId = merchantWalletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            OrderReference = orderReference;
            Description = description;
        }
    }
    
    public class PaymentCompletedEvent : DomainEvent
    {
        public Guid PaymentId { get; }
        public Guid TransactionId { get; }
        public DateTime CompletedAt { get; }
        
        public PaymentCompletedEvent(
            Guid paymentId,
            Guid transactionId,
            DateTime completedAt)
        {
            PaymentId = paymentId;
            TransactionId = transactionId;
            CompletedAt = completedAt;
        }
    }
    
    public class PaymentFailedEvent : DomainEvent
    {
        public Guid PaymentId { get; }
        public string Reason { get; }
        
        public PaymentFailedEvent(Guid paymentId, string reason)
        {
            PaymentId = paymentId;
            Reason = reason;
        }
    }
}