using System;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class WalletTransaction
    {
        public Guid Id { get; private set; }
        public TransactionType Type { get; private set; }
        public Money Amount { get; private set; }
        public Guid? SourceWalletId { get; private set; }
        public Guid? DestinationWalletId { get; private set; }
        public string Source { get; private set; }
        public string Reference { get; private set; }
        public DateTime Timestamp { get; private set; }
        public TransactionStatus Status { get; private set; }
        
        // For EF Core
        private WalletTransaction() { }
        
        public WalletTransaction(
            Guid id,
            TransactionType type,
            Money amount,
            Guid? sourceWalletId,
            Guid? destinationWalletId,
            string source,
            string reference,
            DateTime timestamp,
            TransactionStatus status)
        {
            Id = id;
            Type = type;
            Amount = amount;
            SourceWalletId = sourceWalletId;
            DestinationWalletId = destinationWalletId;
            Source = source;
            Reference = reference;
            Timestamp = timestamp;
            Status = status;
        }
        
        public void MarkAsCompleted()
        {
            Status = TransactionStatus.Completed;
        }
        
        public void MarkAsFailed(string reason)
        {
            Status = TransactionStatus.Failed;
        }
    }
    
    public enum TransactionType
    {
        TopUp,
        Transfer,
        Withdrawal,
        Payment,
        Refund,
        Fee,
        Bonus
    }
    
    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed,
        Cancelled
    }
}