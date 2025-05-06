using System;

namespace WalletService.Domain.Events
{
    public abstract class DomainEvent
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }
        
        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
        }
    }
    
    public class WalletCreatedEvent : DomainEvent
    {
        public Guid WalletId { get; }
        public Guid AccountId { get; }
        public string CurrencyCode { get; }
        
        public WalletCreatedEvent(Guid walletId, Guid accountId, string currencyCode)
        {
            WalletId = walletId;
            AccountId = accountId;
            CurrencyCode = currencyCode;
        }
    }
    
    public class FundsAddedEvent : DomainEvent
    {
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string Source { get; }
        public string Reference { get; }
        public decimal NewBalance { get; }
        public Guid TransactionId { get; }
        
        public FundsAddedEvent(
            Guid walletId,
            decimal amount,
            string currencyCode,
            string source,
            string reference,
            decimal newBalance,
            Guid transactionId)
        {
            WalletId = walletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            Source = source;
            Reference = reference;
            NewBalance = newBalance;
            TransactionId = transactionId;
        }
    }
    
    public class FundsTransferredEvent : DomainEvent
    {
        public Guid SourceWalletId { get; }
        public Guid DestinationWalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string Reference { get; }
        public decimal NewBalance { get; }
        public Guid TransactionId { get; }
        
        public FundsTransferredEvent(
            Guid sourceWalletId,
            Guid destinationWalletId,
            decimal amount,
            string currencyCode,
            string reference,
            decimal newBalance,
            Guid transactionId)
        {
            SourceWalletId = sourceWalletId;
            DestinationWalletId = destinationWalletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            Reference = reference;
            NewBalance = newBalance;
            TransactionId = transactionId;
        }
    }
    
    public class FundsReceivedEvent : DomainEvent
    {
        public Guid WalletId { get; }
        public Guid SourceWalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string Reference { get; }
        public decimal NewBalance { get; }
        public Guid TransactionId { get; }
        
        public FundsReceivedEvent(
            Guid walletId,
            Guid sourceWalletId,
            decimal amount,
            string currencyCode,
            string reference,
            decimal newBalance,
            Guid transactionId)
        {
            WalletId = walletId;
            SourceWalletId = sourceWalletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            Reference = reference;
            NewBalance = newBalance;
            TransactionId = transactionId;
        }
    }
    
    public class FundsWithdrawnEvent : DomainEvent
    {
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string Destination { get; }
        public string Reference { get; }
        public decimal NewBalance { get; }
        public Guid TransactionId { get; }
        
        public FundsWithdrawnEvent(
            Guid walletId,
            decimal amount,
            string currencyCode,
            string destination,
            string reference,
            decimal newBalance,
            Guid transactionId)
        {
            WalletId = walletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            Destination = destination;
            Reference = reference;
            NewBalance = newBalance;
            TransactionId = transactionId;
        }
    }
    
    public class WalletDeactivatedEvent : DomainEvent
    {
        public Guid WalletId { get; }
        
        public WalletDeactivatedEvent(Guid walletId)
        {
            WalletId = walletId;
        }
    }
}