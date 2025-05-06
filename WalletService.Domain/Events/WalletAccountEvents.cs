using System;

namespace WalletService.Domain.Events
{
    public class WalletAccountCreatedEvent : DomainEvent
    {
        public Guid AccountId { get; }
        public string Name { get; }
        public string Type { get; }
        public string KycLevel { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public string PrimaryCurrency { get; }
        
        public WalletAccountCreatedEvent(
            Guid accountId, 
            string name, 
            string type, 
            string kycLevel,
            string email,
            string phoneNumber,
            string primaryCurrency)
        {
            AccountId = accountId;
            Name = name;
            Type = type;
            KycLevel = kycLevel;
            Email = email;
            PhoneNumber = phoneNumber;
            PrimaryCurrency = primaryCurrency;
        }
    }
    
    public class WalletAddedToAccountEvent : DomainEvent
    {
        public Guid AccountId { get; }
        public Guid WalletId { get; }
        
        public WalletAddedToAccountEvent(Guid accountId, Guid walletId)
        {
            AccountId = accountId;
            WalletId = walletId;
        }
    }
    
    public class KycLevelChangedEvent : DomainEvent
    {
        public Guid AccountId { get; }
        public string OldLevel { get; }
        public string NewLevel { get; }
        public string DocumentReference { get; }
        
        public KycLevelChangedEvent(Guid accountId, string oldLevel, string newLevel, string documentReference)
        {
            AccountId = accountId;
            OldLevel = oldLevel;
            NewLevel = newLevel;
            DocumentReference = documentReference;
        }
    }
    
    public class TransactionLimitsUpdatedEvent : DomainEvent
    {
        public Guid AccountId { get; }
        public decimal DailyLimit { get; }
        public decimal MonthlyLimit { get; }
        
        public TransactionLimitsUpdatedEvent(Guid accountId, decimal dailyLimit, decimal monthlyLimit)
        {
            AccountId = accountId;
            DailyLimit = dailyLimit;
            MonthlyLimit = monthlyLimit;
        }
    }
    
    public class WalletAccountClosedEvent : DomainEvent
    {
        public Guid AccountId { get; }
        
        public WalletAccountClosedEvent(Guid accountId)
        {
            AccountId = accountId;
        }
    }
    
    public class WalletAccountSuspendedEvent : DomainEvent
    {
        public Guid AccountId { get; }
        public string Reason { get; }
        
        public WalletAccountSuspendedEvent(Guid accountId, string reason)
        {
            AccountId = accountId;
            Reason = reason;
        }
    }
    
    public class WalletAccountReactivatedEvent : DomainEvent
    {
        public Guid AccountId { get; }
        
        public WalletAccountReactivatedEvent(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}