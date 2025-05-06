using System;
using System.Collections.Generic;

namespace WalletService.Domain.Events
{
    public class RiskProfileCreatedEvent : DomainEvent
    {
        public Guid ProfileId { get; }
        public Guid AccountId { get; }
        public string RiskLevel { get; }
        public Dictionary<string, decimal> TransactionLimits { get; }
        public Dictionary<string, decimal> DailyLimits { get; }
        public Dictionary<string, decimal> MonthlyLimits { get; }
        
        public RiskProfileCreatedEvent(
            Guid profileId,
            Guid accountId,
            string riskLevel,
            Dictionary<string, decimal> transactionLimits,
            Dictionary<string, decimal> dailyLimits,
            Dictionary<string, decimal> monthlyLimits)
        {
            ProfileId = profileId;
            AccountId = accountId;
            RiskLevel = riskLevel;
            TransactionLimits = transactionLimits;
            DailyLimits = dailyLimits;
            MonthlyLimits = monthlyLimits;
        }
    }
    
    public class RiskLevelChangedEvent : DomainEvent
    {
        public Guid ProfileId { get; }
        public Guid AccountId { get; }
        public string OldLevel { get; }
        public string NewLevel { get; }
        public string Reason { get; }
        public Dictionary<string, decimal> TransactionLimits { get; }
        public Dictionary<string, decimal> DailyLimits { get; }
        public Dictionary<string, decimal> MonthlyLimits { get; }
        
        public RiskLevelChangedEvent(
            Guid profileId,
            Guid accountId,
            string oldLevel,
            string newLevel,
            string reason,
            Dictionary<string, decimal> transactionLimits,
            Dictionary<string, decimal> dailyLimits,
            Dictionary<string, decimal> monthlyLimits)
        {
            ProfileId = profileId;
            AccountId = accountId;
            OldLevel = oldLevel;
            NewLevel = newLevel;
            Reason = reason;
            TransactionLimits = transactionLimits;
            DailyLimits = dailyLimits;
            MonthlyLimits = monthlyLimits;
        }
    }
    
    public class AccountFrozenEvent : DomainEvent
    {
        public Guid ProfileId { get; }
        public Guid AccountId { get; }
        public string Reason { get; }
        public DateTime FrozenAt { get; }
        
        public AccountFrozenEvent(
            Guid profileId,
            Guid accountId,
            string reason,
            DateTime frozenAt)
        {
            ProfileId = profileId;
            AccountId = accountId;
            Reason = reason;
            FrozenAt = frozenAt;
        }
    }
    
    public class AccountUnfrozenEvent : DomainEvent
    {
        public Guid ProfileId { get; }
        public Guid AccountId { get; }
        public string Reason { get; }
        public DateTime PreviouslyFrozenAt { get; }
        
        public AccountUnfrozenEvent(
            Guid profileId,
            Guid accountId,
            string reason,
            DateTime previouslyFrozenAt)
        {
            ProfileId = profileId;
            AccountId = accountId;
            Reason = reason;
            PreviouslyFrozenAt = previouslyFrozenAt;
        }
    }
    
    public class TransactionLimitUsageUpdatedEvent : DomainEvent
    {
        public Guid ProfileId { get; }
        public Guid AccountId { get; }
        public string Currency { get; }
        public decimal DailyUsage { get; }
        public decimal MonthlyUsage { get; }
        
        public TransactionLimitUsageUpdatedEvent(
            Guid profileId,
            Guid accountId,
            string currency,
            decimal dailyUsage,
            decimal monthlyUsage)
        {
            ProfileId = profileId;
            AccountId = accountId;
            Currency = currency;
            DailyUsage = dailyUsage;
            MonthlyUsage = monthlyUsage;
        }
    }
    
    public class DailyLimitsResetEvent : DomainEvent
    {
        public Guid ProfileId { get; }
        public Guid AccountId { get; }
        
        public DailyLimitsResetEvent(Guid profileId, Guid accountId)
        {
            ProfileId = profileId;
            AccountId = accountId;
        }
    }
    
    public class MonthlyLimitsResetEvent : DomainEvent
    {
        public Guid ProfileId { get; }
        public Guid AccountId { get; }
        
        public MonthlyLimitsResetEvent(Guid profileId, Guid accountId)
        {
            ProfileId = profileId;
            AccountId = accountId;
        }
    }
}