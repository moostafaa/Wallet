using System;
using System.Collections.Generic;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class CashbackRule
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Percentage { get; private set; }
        public Money MinTransactionAmount { get; private set; }
        public Money MaxCashbackAmount { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public string[] EligibleTransactionTypes { get; private set; }
        
        private CashbackRule() { }
        
        public CashbackRule(
            Guid id,
            string name,
            string description,
            decimal percentage,
            Money minTransactionAmount,
            Money maxCashbackAmount,
            DateTime startDate,
            DateTime? endDate,
            string[] eligibleTransactionTypes)
        {
            if (percentage <= 0 || percentage > 100)
                throw new ArgumentException("Percentage must be between 0 and 100");
                
            Id = id;
            Name = name;
            Description = description;
            Percentage = percentage;
            MinTransactionAmount = minTransactionAmount;
            MaxCashbackAmount = maxCashbackAmount;
            IsActive = true;
            StartDate = startDate;
            EndDate = endDate;
            EligibleTransactionTypes = eligibleTransactionTypes;
            
            AddDomainEvent(new CashbackRuleCreatedEvent(
                Id,
                Name,
                Description,
                Percentage,
                MinTransactionAmount.Amount,
                MinTransactionAmount.Currency.Code,
                MaxCashbackAmount.Amount,
                MaxCashbackAmount.Currency.Code,
                StartDate,
                EndDate,
                EligibleTransactionTypes));
        }
        
        public bool IsEligibleForTransaction(
            Money transactionAmount,
            string transactionType,
            DateTime transactionDate)
        {
            if (!IsActive)
                return false;
                
            if (transactionAmount.Currency != MinTransactionAmount.Currency)
                return false;
                
            if (transactionAmount.Amount < MinTransactionAmount.Amount)
                return false;
                
            if (transactionDate < StartDate)
                return false;
                
            if (EndDate.HasValue && transactionDate > EndDate.Value)
                return false;
                
            return Array.Exists(EligibleTransactionTypes, t => t == transactionType);
        }
        
        public Money CalculateCashback(Money transactionAmount)
        {
            var cashbackAmount = transactionAmount.Multiply(Percentage / 100);
            
            if (cashbackAmount.Amount > MaxCashbackAmount.Amount)
                return MaxCashbackAmount;
                
            return cashbackAmount;
        }
        
        public void Deactivate()
        {
            if (!IsActive)
                return;
                
            IsActive = false;
            EndDate = DateTime.UtcNow;
            
            AddDomainEvent(new CashbackRuleDeactivatedEvent(Id));
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