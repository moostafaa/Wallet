using System;

namespace WalletService.Domain.Events
{
    public class TransactionCreatedEvent : DomainEvent
    {
        public Guid TransactionId { get; }
        public string Type { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public Guid? SourceWalletId { get; }
        public Guid? DestinationWalletId { get; }
        public string Source { get; }
        public string Reference { get; }
        
        public TransactionCreatedEvent(
            Guid transactionId,
            string type,
            decimal amount,
            string currencyCode,
            Guid? sourceWalletId,
            Guid? destinationWalletId,
            string source,
            string reference)
        {
            TransactionId = transactionId;
            Type = type;
            Amount = amount;
            CurrencyCode = currencyCode;
            SourceWalletId = sourceWalletId;
            DestinationWalletId = destinationWalletId;
            Source = source;
            Reference = reference;
        }
    }
    
    public class TransactionCompletedEvent : DomainEvent
    {
        public Guid TransactionId { get; }
        
        public TransactionCompletedEvent(Guid transactionId)
        {
            TransactionId = transactionId;
        }
    }
    
    public class TransactionFailedEvent : DomainEvent
    {
        public Guid TransactionId { get; }
        public string Reason { get; }
        
        public TransactionFailedEvent(Guid transactionId, string reason)
        {
            TransactionId = transactionId;
            Reason = reason;
        }
    }
    
    public class TransactionReversedEvent : DomainEvent
    {
        public Guid TransactionId { get; }
        public string Reason { get; }
        
        public TransactionReversedEvent(Guid transactionId, string reason)
        {
            TransactionId = transactionId;
            Reason = reason;
        }
    }
}