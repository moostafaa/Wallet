using System;

namespace WalletService.Domain.Events
{
    public class TopUpRequestCreatedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string Source { get; }
        public string ExternalReference { get; }
        
        public TopUpRequestCreatedEvent(
            Guid requestId,
            Guid walletId,
            decimal amount,
            string currencyCode,
            string source,
            string externalReference)
        {
            RequestId = requestId;
            WalletId = walletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            Source = source;
            ExternalReference = externalReference;
        }
    }
    
    public class TopUpCompletedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string TransactionReference { get; }
        
        public TopUpCompletedEvent(
            Guid requestId,
            Guid walletId,
            decimal amount,
            string currencyCode,
            string transactionReference)
        {
            RequestId = requestId;
            WalletId = walletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            TransactionReference = transactionReference;
        }
    }
    
    public class AutoTopUpRuleCreatedEvent : DomainEvent
    {
        public Guid RuleId { get; }
        public Guid WalletId { get; }
        public decimal TriggerAmount { get; }
        public decimal TopUpAmount { get; }
        public string Source { get; }
        
        public AutoTopUpRuleCreatedEvent(
            Guid ruleId,
            Guid walletId,
            decimal triggerAmount,
            decimal topUpAmount,
            string source)
        {
            RuleId = ruleId;
            WalletId = walletId;
            TriggerAmount = triggerAmount;
            TopUpAmount = topUpAmount;
            Source = source;
        }
    }
}