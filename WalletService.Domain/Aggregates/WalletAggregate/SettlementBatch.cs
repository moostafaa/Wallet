```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class SettlementBatch
    {
        private readonly List<SettlementTransaction> _transactions = new List<SettlementTransaction>();
        
        public Guid Id { get; private set; }
        public Guid MerchantId { get; private set; }
        public Currency Currency { get; private set; }
        public Money TotalAmount { get; private set; }
        public string BankAccountNumber { get; private set; }
        public string BankRoutingNumber { get; private set; }
        public string BankName { get; private set; }
        public string AccountHolderName { get; private set; }
        public SettlementStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ProcessedAt { get; private set; }
        public string Reference { get; private set; }
        
        public IReadOnlyCollection<SettlementTransaction> Transactions => _transactions.AsReadOnly();
        
        private SettlementBatch() { }
        
        public SettlementBatch(
            Guid id,
            Guid merchantId,
            Currency currency,
            string bankAccountNumber,
            string bankRoutingNumber,
            string bankName,
            string accountHolderName)
        {
            Id = id;
            MerchantId = merchantId;
            Currency = currency;
            TotalAmount = Money.Zero(currency);
            BankAccountNumber = bankAccountNumber;
            BankRoutingNumber = bankRoutingNumber;
            BankName = bankName;
            AccountHolderName = accountHolderName;
            Status = SettlementStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new SettlementBatchCreatedEvent(
                Id,
                MerchantId,
                Currency.Code,
                BankAccountNumber,
                BankRoutingNumber,
                BankName,
                AccountHolderName));
        }
        
        public void AddTransaction(Guid transactionId, Money amount)
        {
            if (amount.Currency != Currency)
                throw new InvalidOperationException("Transaction currency must match batch currency");
                
            var transaction = new SettlementTransaction(transactionId, amount);
            _transactions.Add(transaction);
            TotalAmount = TotalAmount.Add(amount);
            
            AddDomainEvent(new TransactionAddedToSettlementEvent(
                Id,
                transactionId,
                amount.Amount));
        }
        
        public void Process(string reference)
        {
            if (Status != SettlementStatus.Pending)
                throw new InvalidOperationException("Can only process pending settlements");
                
            if (!_transactions.Any())
                throw new InvalidOperationException("Cannot process empty settlement batch");
                
            Status = SettlementStatus.Processing;
            Reference = reference;
            
            AddDomainEvent(new SettlementBatchProcessingEvent(
                Id,
                TotalAmount.Amount,
                Reference));
        }
        
        public void Complete()
        {
            if (Status != SettlementStatus.Processing)
                throw new InvalidOperationException("Can only complete processing settlements");
                
            Status = SettlementStatus.Completed;
            ProcessedAt = DateTime.UtcNow;
            
            AddDomainEvent(new SettlementBatchCompletedEvent(
                Id,
                ProcessedAt.Value));
        }
        
        public void Fail(string reason)
        {
            if (Status != SettlementStatus.Processing)
                throw new InvalidOperationException("Can only fail processing settlements");
                
            Status = SettlementStatus.Failed;
            ProcessedAt = DateTime.UtcNow;
            
            AddDomainEvent(new SettlementBatchFailedEvent(
                Id,
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
    
    public class SettlementTransaction
    {
        public Guid TransactionId { get; private set; }
        public Money Amount { get; private set; }
        
        public SettlementTransaction(Guid transactionId, Money amount)
        {
            TransactionId = transactionId;
            Amount = amount;
        }
    }
    
    public enum SettlementStatus
    {
        Pending,
        Processing,
        Completed,
        Failed
    }
}
```