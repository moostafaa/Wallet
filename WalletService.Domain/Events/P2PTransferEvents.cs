using System;

namespace WalletService.Domain.Events
{
    public class MoneyRequestCreatedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public Guid RequesterId { get; }
        public Guid RequesteeId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string Description { get; }
        
        public MoneyRequestCreatedEvent(
            Guid requestId,
            Guid requesterId,
            Guid requesteeId,
            decimal amount,
            string currencyCode,
            string description)
        {
            RequestId = requestId;
            RequesterId = requesterId;
            RequesteeId = requesteeId;
            Amount = amount;
            CurrencyCode = currencyCode;
            Description = description;
        }
    }
    
    public class MoneyRequestAcceptedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public Guid TransactionId { get; }
        
        public MoneyRequestAcceptedEvent(Guid requestId, Guid transactionId)
        {
            RequestId = requestId;
            TransactionId = transactionId;
        }
    }
    
    public class MoneyRequestRejectedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public string Reason { get; }
        
        public MoneyRequestRejectedEvent(Guid requestId, string reason)
        {
            RequestId = requestId;
            Reason = reason;
        }
    }
    
    public class RecurringTransferCreatedEvent : DomainEvent
    {
        public Guid TransferId { get; }
        public Guid SourceWalletId { get; }
        public Guid DestinationWalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string Description { get; }
        public string Frequency { get; }
        public DateTime NextExecutionDate { get; }
        
        public RecurringTransferCreatedEvent(
            Guid transferId,
            Guid sourceWalletId,
            Guid destinationWalletId,
            decimal amount,
            string currencyCode,
            string description,
            string frequency,
            DateTime nextExecutionDate)
        {
            TransferId = transferId;
            SourceWalletId = sourceWalletId;
            DestinationWalletId = destinationWalletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            Description = description;
            Frequency = frequency;
            NextExecutionDate = nextExecutionDate;
        }
    }
    
    public class RecurringTransferExecutedEvent : DomainEvent
    {
        public Guid TransferId { get; }
        public Guid TransactionId { get; }
        public DateTime NextExecutionDate { get; }
        
        public RecurringTransferExecutedEvent(
            Guid transferId,
            Guid transactionId,
            DateTime nextExecutionDate)
        {
            TransferId = transferId;
            TransactionId = transactionId;
            NextExecutionDate = nextExecutionDate;
        }
    }
    
    public class RecurringTransferDeactivatedEvent : DomainEvent
    {
        public Guid TransferId { get; }
        
        public RecurringTransferDeactivatedEvent(Guid transferId)
        {
            TransferId = transferId;
        }
    }
}