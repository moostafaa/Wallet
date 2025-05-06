```csharp
using System;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class WithdrawalRequest
    {
        public Guid Id { get; private set; }
        public Guid WalletId { get; private set; }
        public Money Amount { get; private set; }
        public string BankAccountNumber { get; private set; }
        public string BankRoutingNumber { get; private set; }
        public string BankName { get; private set; }
        public string AccountHolderName { get; private set; }
        public WithdrawalStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string ProcessedBy { get; private set; }
        public Guid? TransactionId { get; private set; }
        
        private WithdrawalRequest() { }
        
        public WithdrawalRequest(
            Guid id,
            Guid walletId,
            Money amount,
            string bankAccountNumber,
            string bankRoutingNumber,
            string bankName,
            string accountHolderName)
        {
            Id = id;
            WalletId = walletId;
            Amount = amount;
            BankAccountNumber = bankAccountNumber;
            BankRoutingNumber = bankRoutingNumber;
            BankName = bankName;
            AccountHolderName = accountHolderName;
            Status = WithdrawalStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new WithdrawalRequestCreatedEvent(
                Id,
                WalletId,
                Amount.Amount,
                Amount.Currency.Code,
                BankAccountNumber,
                BankRoutingNumber,
                BankName,
                AccountHolderName));
        }
        
        public void Approve(string approvedBy, Guid transactionId)
        {
            if (Status != WithdrawalStatus.Pending)
                throw new InvalidOperationException("Can only approve pending withdrawals");
                
            Status = WithdrawalStatus.Approved;
            ProcessedAt = DateTime.UtcNow;
            ProcessedBy = approvedBy;
            TransactionId = transactionId;
            
            AddDomainEvent(new WithdrawalRequestApprovedEvent(
                Id,
                TransactionId.Value,
                ProcessedBy));
        }
        
        public void Reject(string rejectedBy, string reason)
        {
            if (Status != WithdrawalStatus.Pending)
                throw new InvalidOperationException("Can only reject pending withdrawals");
                
            Status = WithdrawalStatus.Rejected;
            ProcessedAt = DateTime.UtcNow;
            ProcessedBy = rejectedBy;
            
            AddDomainEvent(new WithdrawalRequestRejectedEvent(
                Id,
                rejectedBy,
                reason));
        }
        
        private List<object> _domainEvents = new List<object>();
        public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();
        
        public void AddDomainEvent(object eventItem)
        {
            _domainEvents.Add(eventItem);
        }
        
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
    
    public enum WithdrawalStatus
    {
        Pending,
        Approved,
        Rejected,
        Failed
    }
}
```