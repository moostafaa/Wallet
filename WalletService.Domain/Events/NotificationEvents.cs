using System;

namespace WalletService.Domain.Events
{
    public class TransactionNotificationEvent : DomainEvent
    {
        public Guid WalletId { get; }
        public Guid AccountId { get; }
        public string Type { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string Description { get; }
        public string NotificationChannel { get; }
        
        public TransactionNotificationEvent(
            Guid walletId,
            Guid accountId,
            string type,
            decimal amount,
            string currencyCode,
            string description,
            string notificationChannel)
        {
            WalletId = walletId;
            AccountId = accountId;
            Type = type;
            Amount = amount;
            CurrencyCode = currencyCode;
            Description = description;
            NotificationChannel = notificationChannel;
        }
    }
    
    public class WalletLimitNotificationEvent : DomainEvent
    {
        public Guid WalletId { get; }
        public Guid AccountId { get; }
        public string LimitType { get; }
        public decimal CurrentUsage { get; }
        public decimal Limit { get; }
        public string NotificationChannel { get; }
        
        public WalletLimitNotificationEvent(
            Guid walletId,
            Guid accountId,
            string limitType,
            decimal currentUsage,
            decimal limit,
            string notificationChannel)
        {
            WalletId = walletId;
            AccountId = accountId;
            LimitType = limitType;
            CurrentUsage = currentUsage;
            Limit = limit;
            NotificationChannel = notificationChannel;
        }
    }
}