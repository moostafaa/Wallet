using System;

namespace WalletService.Domain.Events
{
    public class CashbackRuleCreatedEvent : DomainEvent
    {
        public Guid RuleId { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal Percentage { get; }
        public decimal MinTransactionAmount { get; }
        public string CurrencyCode { get; }
        public decimal MaxCashbackAmount { get; }
        public string MaxCashbackCurrencyCode { get; }
        public DateTime StartDate { get; }
        public DateTime? EndDate { get; }
        public string[] EligibleTransactionTypes { get; }
        
        public CashbackRuleCreatedEvent(
            Guid ruleId,
            string name,
            string description,
            decimal percentage,
            decimal minTransactionAmount,
            string currencyCode,
            decimal maxCashbackAmount,
            string maxCashbackCurrencyCode,
            DateTime startDate,
            DateTime? endDate,
            string[] eligibleTransactionTypes)
        {
            RuleId = ruleId;
            Name = name;
            Description = description;
            Percentage = percentage;
            MinTransactionAmount = minTransactionAmount;
            CurrencyCode = currencyCode;
            MaxCashbackAmount = maxCashbackAmount;
            MaxCashbackCurrencyCode = maxCashbackCurrencyCode;
            StartDate = startDate;
            EndDate = endDate;
            EligibleTransactionTypes = eligibleTransactionTypes;
        }
    }
    
    public class CashbackRuleDeactivatedEvent : DomainEvent
    {
        public Guid RuleId { get; }
        
        public CashbackRuleDeactivatedEvent(Guid ruleId)
        {
            RuleId = ruleId;
        }
    }
    
    public class CashbackAwardedEvent : DomainEvent
    {
        public Guid WalletId { get; }
        public Guid RuleId { get; }
        public Guid TransactionId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        
        public CashbackAwardedEvent(
            Guid walletId,
            Guid ruleId,
            Guid transactionId,
            decimal amount,
            string currencyCode)
        {
            WalletId = walletId;
            RuleId = ruleId;
            TransactionId = transactionId;
            Amount = amount;
            CurrencyCode = currencyCode;
        }
    }
    
    public class ReferralBonusCreatedEvent : DomainEvent
    {
        public Guid BonusId { get; }
        public Guid ReferrerId { get; }
        public Guid ReferredUserId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        
        public ReferralBonusCreatedEvent(
            Guid bonusId,
            Guid referrerId,
            Guid referredUserId,
            decimal amount,
            string currencyCode)
        {
            BonusId = bonusId;
            ReferrerId = referrerId;
            ReferredUserId = referredUserId;
            Amount = amount;
            CurrencyCode = currencyCode;
        }
    }
    
    public class ReferralBonusPaidEvent : DomainEvent
    {
        public Guid BonusId { get; }
        public Guid ReferrerId { get; }
        public Guid TransactionId { get; }
        
        public ReferralBonusPaidEvent(
            Guid bonusId,
            Guid referrerId,
            Guid transactionId)
        {
            BonusId = bonusId;
            ReferrerId = referrerId;
            TransactionId = transactionId;
        }
    }
    
    public class ReferralBonusCancelledEvent : DomainEvent
    {
        public Guid BonusId { get; }
        public string Reason { get; }
        
        public ReferralBonusCancelledEvent(Guid bonusId, string reason)
        {
            BonusId = bonusId;
            Reason = reason;
        }
    }
}