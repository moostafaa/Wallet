using System;
using System.Collections.Generic;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class FeeRule
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public FeeType Type { get; private set; }
        public decimal Percentage { get; private set; }
        public Money FixedAmount { get; private set; }
        public string[] ApplicableTransactionTypes { get; private set; }
        public string[] ApplicableMerchantTypes { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? DeactivatedAt { get; private set; }
        
        private FeeRule() { }
        
        public FeeRule(
            Guid id,
            string name,
            string description,
            FeeType type,
            decimal percentage,
            Money fixedAmount,
            string[] applicableTransactionTypes,
            string[] applicableMerchantTypes)
        {
            Id = id;
            Name = name;
            Description = description;
            Type = type;
            Percentage = percentage;
            FixedAmount = fixedAmount;
            ApplicableTransactionTypes = applicableTransactionTypes;
            ApplicableMerchantTypes = applicableMerchantTypes;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new FeeRuleCreatedEvent(
                Id,
                Name,
                Description,
                Type.ToString(),
                Percentage,
                FixedAmount.Amount,
                FixedAmount.Currency.Code,
                ApplicableTransactionTypes,
                ApplicableMerchantTypes));
        }
        
        public void Deactivate()
        {
            if (!IsActive)
                return;
                
            IsActive = false;
            DeactivatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new FeeRuleDeactivatedEvent(Id));
        }
        
        public Money CalculateFee(Money transactionAmount)
        {
            if (!IsActive)
                return Money.Zero(transactionAmount.Currency);
                
            switch (Type)
            {
                case FeeType.Fixed:
                    return FixedAmount;
                    
                case FeeType.Percentage:
                    return transactionAmount.Multiply(Percentage / 100);
                    
                case FeeType.FixedPlusPercentage:
                    var percentageFee = transactionAmount.Multiply(Percentage / 100);
                    return FixedAmount.Add(percentageFee);
                    
                default:
                    throw new InvalidOperationException($"Unsupported fee type: {Type}");
            }
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
    
    public enum FeeType
    {
        Fixed,
        Percentage,
        FixedPlusPercentage
    }
}