using System;
using System.Collections.Generic;
using System.Linq;
using WalletService.Domain.Events;
using WalletService.Domain.Exceptions;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class Wallet
    {
        private readonly List<WalletTransaction> _transactions = new List<WalletTransaction>();
        
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public Currency Currency { get; private set; }
        public Money Balance { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DeactivatedAt { get; private set; }
        
        public IReadOnlyCollection<WalletTransaction> Transactions => _transactions.AsReadOnly();

        // For EF Core
        private Wallet() { }

        public Wallet(Guid id, Guid accountId, Currency currency)
        {
            Id = id;
            AccountId = accountId;
            Currency = currency;
            Balance = Money.Zero(currency);
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new WalletCreatedEvent(Id, AccountId, Currency.Code));
        }

        public void AddFunds(Money amount, string source, string reference)
        {
            if (!IsActive)
                throw new WalletInactiveException(Id);
                
            if (amount.Currency != Currency)
                throw new CurrencyMismatchException(amount.Currency.Code, Currency.Code);
                
            if (amount.Amount <= 0)
                throw new InvalidAmountException("Amount must be greater than zero");
                
            Balance = Balance.Add(amount);
            
            var transaction = CreateTransaction(
                TransactionType.TopUp,
                amount,
                null,
                Id,
                source,
                reference);
                
            _transactions.Add(transaction);
            
            AddDomainEvent(new FundsAddedEvent(
                Id, 
                amount.Amount, 
                Currency.Code, 
                source, 
                reference, 
                Balance.Amount, 
                transaction.Id));
        }
        
        public void TransferFunds(Money amount, Guid destinationWalletId, string reference)
        {
            if (!IsActive)
                throw new WalletInactiveException(Id);
                
            if (amount.Currency != Currency)
                throw new CurrencyMismatchException(amount.Currency.Code, Currency.Code);
                
            if (amount.Amount <= 0)
                throw new InvalidAmountException("Amount must be greater than zero");
                
            if (Balance.Amount < amount.Amount)
                throw new InsufficientFundsException(Id, amount.Amount, Balance.Amount);
                
            Balance = Balance.Subtract(amount);
            
            var transaction = CreateTransaction(
                TransactionType.Transfer,
                amount.Negate(),
                Id,
                destinationWalletId,
                "wallet",
                reference);
                
            _transactions.Add(transaction);
            
            AddDomainEvent(new FundsTransferredEvent(
                Id,
                destinationWalletId,
                amount.Amount,
                Currency.Code,
                reference,
                Balance.Amount,
                transaction.Id));
        }
        
        public void ReceiveFunds(Money amount, Guid sourceWalletId, string reference, Guid transactionId)
        {
            if (!IsActive)
                throw new WalletInactiveException(Id);
                
            if (amount.Currency != Currency)
                throw new CurrencyMismatchException(amount.Currency.Code, Currency.Code);
                
            if (amount.Amount <= 0)
                throw new InvalidAmountException("Amount must be greater than zero");
                
            Balance = Balance.Add(amount);
            
            var transaction = new WalletTransaction(
                transactionId,
                TransactionType.Transfer,
                amount,
                sourceWalletId,
                Id,
                "wallet",
                reference,
                DateTime.UtcNow,
                TransactionStatus.Completed);
                
            _transactions.Add(transaction);
            
            AddDomainEvent(new FundsReceivedEvent(
                Id,
                sourceWalletId,
                amount.Amount,
                Currency.Code,
                reference,
                Balance.Amount,
                transaction.Id));
        }
        
        public void WithdrawFunds(Money amount, string destination, string reference)
        {
            if (!IsActive)
                throw new WalletInactiveException(Id);
                
            if (amount.Currency != Currency)
                throw new CurrencyMismatchException(amount.Currency.Code, Currency.Code);
                
            if (amount.Amount <= 0)
                throw new InvalidAmountException("Amount must be greater than zero");
                
            if (Balance.Amount < amount.Amount)
                throw new InsufficientFundsException(Id, amount.Amount, Balance.Amount);
                
            Balance = Balance.Subtract(amount);
            
            var transaction = CreateTransaction(
                TransactionType.Withdrawal,
                amount.Negate(),
                Id,
                null,
                destination,
                reference);
                
            _transactions.Add(transaction);
            
            AddDomainEvent(new FundsWithdrawnEvent(
                Id,
                amount.Amount,
                Currency.Code,
                destination,
                reference,
                Balance.Amount,
                transaction.Id));
        }
        
        public void Deactivate()
        {
            if (!IsActive)
                throw new WalletInactiveException(Id);
                
            IsActive = false;
            DeactivatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new WalletDeactivatedEvent(Id));
        }
        
        public void ApplyEvent(WalletCreatedEvent @event)
        {
            Id = @event.WalletId;
            AccountId = @event.AccountId;
            Currency = Currency.FromCode(@event.CurrencyCode);
            Balance = Money.Zero(Currency);
            IsActive = true;
            CreatedAt = @event.Timestamp;
        }
        
        public void ApplyEvent(FundsAddedEvent @event)
        {
            Balance = new Money(@event.NewBalance, Currency);
            
            var transaction = new WalletTransaction(
                @event.TransactionId,
                TransactionType.TopUp,
                new Money(@event.Amount, Currency),
                null,
                Id,
                @event.Source,
                @event.Reference,
                @event.Timestamp,
                TransactionStatus.Completed);
                
            _transactions.Add(transaction);
        }
        
        public void ApplyEvent(FundsTransferredEvent @event)
        {
            Balance = new Money(@event.NewBalance, Currency);
            
            var transaction = new WalletTransaction(
                @event.TransactionId,
                TransactionType.Transfer,
                new Money(-@event.Amount, Currency),
                Id,
                @event.DestinationWalletId,
                "wallet",
                @event.Reference,
                @event.Timestamp,
                TransactionStatus.Completed);
                
            _transactions.Add(transaction);
        }
        
        public void ApplyEvent(FundsReceivedEvent @event)
        {
            Balance = new Money(@event.NewBalance, Currency);
            
            var transaction = new WalletTransaction(
                @event.TransactionId,
                TransactionType.Transfer,
                new Money(@event.Amount, Currency),
                @event.SourceWalletId,
                Id,
                "wallet",
                @event.Reference,
                @event.Timestamp,
                TransactionStatus.Completed);
                
            _transactions.Add(transaction);
        }
        
        public void ApplyEvent(FundsWithdrawnEvent @event)
        {
            Balance = new Money(@event.NewBalance, Currency);
            
            var transaction = new WalletTransaction(
                @event.TransactionId,
                TransactionType.Withdrawal,
                new Money(-@event.Amount, Currency),
                Id,
                null,
                @event.Destination,
                @event.Reference,
                @event.Timestamp,
                TransactionStatus.Completed);
                
            _transactions.Add(transaction);
        }
        
        public void ApplyEvent(WalletDeactivatedEvent @event)
        {
            IsActive = false;
            DeactivatedAt = @event.Timestamp;
        }
        
        private WalletTransaction CreateTransaction(
            TransactionType type,
            Money amount,
            Guid? sourceWalletId,
            Guid? destinationWalletId,
            string source,
            string reference)
        {
            return new WalletTransaction(
                Guid.NewGuid(),
                type,
                amount,
                sourceWalletId,
                destinationWalletId,
                source,
                reference,
                DateTime.UtcNow,
                TransactionStatus.Completed);
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
}