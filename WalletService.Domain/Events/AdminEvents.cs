using System;

namespace WalletService.Domain.Events
{
    public class FeeRuleCreatedEvent : DomainEvent
    {
        public Guid RuleId { get; }
        public string Name { get; }
        public string Description { get; }
        public string Type { get; }
        public decimal Percentage { get; }
        public decimal FixedAmount { get; }
        public string CurrencyCode { get; }
        public string[] ApplicableTransactionTypes { get; }
        public string[] ApplicableMerchantTypes { get; }
        
        public FeeRuleCreatedEvent(
            Guid ruleId,
            string name,
            string description,
            string type,
            decimal percentage,
            decimal fixedAmount,
            string currencyCode,
            string[] applicableTransactionTypes,
            string[] applicableMerchantTypes)
        {
            RuleId = ruleId;
            Name = name;
            Description = description;
            Type = type;
            Percentage = percentage;
            FixedAmount = fixedAmount;
            CurrencyCode = currencyCode;
            ApplicableTransactionTypes = applicableTransactionTypes;
            ApplicableMerchantTypes = applicableMerchantTypes;
        }
    }
    
    public class FeeRuleDeactivatedEvent : DomainEvent
    {
        public Guid RuleId { get; }
        
        public FeeRuleDeactivatedEvent(Guid ruleId)
        {
            RuleId = ruleId;
        }
    }
    
    public class ExchangeRateCreatedEvent : DomainEvent
    {
        public Guid RateId { get; }
        public string BaseCurrency { get; }
        public string QuoteCurrency { get; }
        public decimal Rate { get; }
        public decimal SpreadPercentage { get; }
        
        public ExchangeRateCreatedEvent(
            Guid rateId,
            string baseCurrency,
            string quoteCurrency,
            decimal rate,
            decimal spreadPercentage)
        {
            RateId = rateId;
            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
            Rate = rate;
            SpreadPercentage = spreadPercentage;
        }
    }
    
    public class ExchangeRateUpdatedEvent : DomainEvent
    {
        public Guid RateId { get; }
        public string BaseCurrency { get; }
        public string QuoteCurrency { get; }
        public decimal OldRate { get; }
        public decimal NewRate { get; }
        public decimal OldSpread { get; }
        public decimal NewSpread { get; }
        
        public ExchangeRateUpdatedEvent(
            Guid rateId,
            string baseCurrency,
            string quoteCurrency,
            decimal oldRate,
            decimal newRate,
            decimal oldSpread,
            decimal newSpread)
        {
            RateId = rateId;
            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
            OldRate = oldRate;
            NewRate = newRate;
            OldSpread = oldSpread;
            NewSpread = newSpread;
        }
    }
}