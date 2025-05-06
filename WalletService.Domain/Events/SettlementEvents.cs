```csharp
using System;

namespace WalletService.Domain.Events
{
    public class SettlementBatchCreatedEvent : DomainEvent
    {
        public Guid BatchId { get; }
        public Guid MerchantId { get; }
        public string CurrencyCode { get; }
        public string BankAccountNumber { get; }
        public string BankRoutingNumber { get; }
        public string BankName { get; }
        public string AccountHolderName { get; }
        
        public SettlementBatchCreatedEvent(
            Guid batchId,
            Guid merchantId,
            string currencyCode,
            string bankAccountNumber,
            string bankRoutingNumber,
            string bankName,
            string accountHolderName)
        {
            BatchId = batchId;
            MerchantId = merchantId;
            CurrencyCode = currencyCode;
            BankAccountNumber = bankAccountNumber;
            BankRoutingNumber = bankRoutingNumber;
            BankName = bankName;
            AccountHolderName = accountHolderName;
        }
    }
    
    public class TransactionAddedToSettlementEvent : DomainEvent
    {
        public Guid BatchId { get; }
        public Guid TransactionId { get; }
        public decimal Amount { get; }
        
        public TransactionAddedToSettlementEvent(
            Guid batchId,
            Guid transactionId,
            decimal amount)
        {
            BatchId = batchId;
            TransactionId = transactionId;
            Amount = amount;
        }
    }
    
    public class SettlementBatchProcessingEvent : DomainEvent
    {
        public Guid BatchId { get; }
        public decimal TotalAmount { get; }
        public string Reference { get; }
        
        public SettlementBatchProcessingEvent(
            Guid batchId,
            decimal totalAmount,
            string reference)
        {
            BatchId = batchId;
            TotalAmount = totalAmount;
            Reference = reference;
        }
    }
    
    public class SettlementBatchCompletedEvent : DomainEvent
    {
        public Guid BatchId { get; }
        public DateTime CompletedAt { get; }
        
        public SettlementBatchCompletedEvent(Guid batchId, DateTime completedAt)
        {
            BatchId = batchId;
            CompletedAt = completedAt;
        }
    }
    
    public class SettlementBatchFailedEvent : DomainEvent
    {
        public Guid BatchId { get; }
        public string Reason { get; }
        
        public SettlementBatchFailedEvent(Guid batchId, string reason)
        {
            BatchId = batchId;
            Reason = reason;
        }
    }
}
```