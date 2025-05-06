using System;
using System.Collections.Generic;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAccountAggregate
{
    public class RiskProfile
    {
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public RiskLevel Level { get; private set; }
        public bool IsFrozen { get; private set; }
        public DateTime? FrozenAt { get; private set; }
        public string FrozenReason { get; private set; }
        public Dictionary<string, decimal> TransactionLimits { get; private set; }
        public Dictionary<string, decimal> DailyLimits { get; private set; }
        public Dictionary<string, decimal> MonthlyLimits { get; private set; }
        public Dictionary<string, decimal> CurrentDailyUsage { get; private set; }
        public Dictionary<string, decimal> CurrentMonthlyUsage { get; private set; }
        public DateTime LastResetDate { get; private set; }
        
        private RiskProfile() { }
        
        public RiskProfile(Guid id, Guid accountId, RiskLevel level)
        {
            Id = id;
            AccountId = accountId;
            Level = level;
            IsFrozen = false;
            TransactionLimits = new Dictionary<string, decimal>();
            DailyLimits = new Dictionary<string, decimal>();
            MonthlyLimits = new Dictionary<string, decimal>();
            CurrentDailyUsage = new Dictionary<string, decimal>();
            CurrentMonthlyUsage = new Dictionary<string, decimal>();
            LastResetDate = DateTime.UtcNow;
            
            SetDefaultLimits();
            
            AddDomainEvent(new RiskProfileCreatedEvent(
                Id,
                AccountId,
                Level.ToString(),
                TransactionLimits,
                DailyLimits,
                MonthlyLimits));
        }
        
        public void UpdateRiskLevel(RiskLevel newLevel, string reason)
        {
            if (newLevel == Level)
                return;
                
            var oldLevel = Level;
            Level = newLevel;
            SetDefaultLimits();
            
            AddDomainEvent(new RiskLevelChangedEvent(
                Id,
                AccountId,
                oldLevel.ToString(),
                newLevel.ToString(),
                reason,
                TransactionLimits,
                DailyLimits,
                MonthlyLimits));
        }
        
        public void FreezeAccount(string reason)
        {
            if (IsFrozen)
                return;
                
            IsFrozen = true;
            FrozenAt = DateTime.UtcNow;
            FrozenReason = reason;
            
            AddDomainEvent(new AccountFrozenEvent(
                Id,
                AccountId,
                reason,
                FrozenAt.Value));
        }
        
        public void UnfreezeAccount(string reason)
        {
            if (!IsFrozen)
                return;
                
            IsFrozen = false;
            var frozenAt = FrozenAt;
            FrozenAt = null;
            FrozenReason = null;
            
            AddDomainEvent(new AccountUnfrozenEvent(
                Id,
                AccountId,
                reason,
                frozenAt.Value));
        }
        
        public bool CanProcessTransaction(Money amount, string transactionType)
        {
            if (IsFrozen)
                return false;
                
            var currency = amount.Currency.Code;
            
            // Check transaction limit
            if (TransactionLimits.TryGetValue(currency, out var transactionLimit) &&
                amount.Amount > transactionLimit)
                return false;
                
            // Check daily limit
            if (DailyLimits.TryGetValue(currency, out var dailyLimit))
            {
                var currentUsage = CurrentDailyUsage.GetValueOrDefault(currency);
                if (currentUsage + amount.Amount > dailyLimit)
                    return false;
            }
            
            // Check monthly limit
            if (MonthlyLimits.TryGetValue(currency, out var monthlyLimit))
            {
                var currentUsage = CurrentMonthlyUsage.GetValueOrDefault(currency);
                if (currentUsage + amount.Amount > monthlyLimit)
                    return false;
            }
            
            return true;
        }
        
        public void RecordTransaction(Money amount)
        {
            var currency = amount.Currency.Code;
            
            // Update daily usage
            if (!CurrentDailyUsage.ContainsKey(currency))
                CurrentDailyUsage[currency] = 0;
            CurrentDailyUsage[currency] += amount.Amount;
            
            // Update monthly usage
            if (!CurrentMonthlyUsage.ContainsKey(currency))
                CurrentMonthlyUsage[currency] = 0;
            CurrentMonthlyUsage[currency] += amount.Amount;
            
            AddDomainEvent(new TransactionLimitUsageUpdatedEvent(
                Id,
                AccountId,
                currency,
                CurrentDailyUsage[currency],
                CurrentMonthlyUsage[currency]));
        }
        
        public void ResetDailyLimits()
        {
            CurrentDailyUsage.Clear();
            LastResetDate = DateTime.UtcNow;
            
            AddDomainEvent(new DailyLimitsResetEvent(Id, AccountId));
        }
        
        public void ResetMonthlyLimits()
        {
            CurrentMonthlyUsage.Clear();
            
            AddDomainEvent(new MonthlyLimitsResetEvent(Id, AccountId));
        }
        
        private void SetDefaultLimits()
        {
            switch (Level)
            {
                case RiskLevel.Low:
                    SetLowRiskLimits();
                    break;
                case RiskLevel.Medium:
                    SetMediumRiskLimits();
                    break;
                case RiskLevel.High:
                    SetHighRiskLimits();
                    break;
            }
        }
        
        private void SetLowRiskLimits()
        {
            TransactionLimits["USD"] = 10000;
            DailyLimits["USD"] = 50000;
            MonthlyLimits["USD"] = 200000;
            
            TransactionLimits["EUR"] = 8500;
            DailyLimits["EUR"] = 42500;
            MonthlyLimits["EUR"] = 170000;
            
            TransactionLimits["GBP"] = 7500;
            DailyLimits["GBP"] = 37500;
            MonthlyLimits["GBP"] = 150000;
        }
        
        private void SetMediumRiskLimits()
        {
            TransactionLimits["USD"] = 5000;
            DailyLimits["USD"] = 25000;
            MonthlyLimits["USD"] = 100000;
            
            TransactionLimits["EUR"] = 4250;
            DailyLimits["EUR"] = 21250;
            MonthlyLimits["EUR"] = 85000;
            
            TransactionLimits["GBP"] = 3750;
            DailyLimits["GBP"] = 18750;
            MonthlyLimits["GBP"] = 75000;
        }
        
        private void SetHighRiskLimits()
        {
            TransactionLimits["USD"] = 1000;
            DailyLimits["USD"] = 5000;
            MonthlyLimits["USD"] = 20000;
            
            TransactionLimits["EUR"] = 850;
            DailyLimits["EUR"] = 4250;
            MonthlyLimits["EUR"] = 17000;
            
            TransactionLimits["GBP"] = 750;
            DailyLimits["GBP"] = 3750;
            MonthlyLimits["GBP"] = 15000;
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
    
    public enum RiskLevel
    {
        Low,
        Medium,
        High
    }
}