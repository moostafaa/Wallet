using System;
using System.Collections.Generic;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class ExchangeRate
    {
        public Guid Id { get; private set; }
        public Currency BaseCurrency { get; private set; }
        public Currency QuoteCurrency { get; private set; }
        public decimal Rate { get; private set; }
        public decimal SpreadPercentage { get; private set; }
        public DateTime LastUpdated { get; private set; }
        
        private ExchangeRate() { }
        
        public ExchangeRate(
            Guid id,
            Currency baseCurrency,
            Currency quoteCurrency,
            decimal rate,
            decimal spreadPercentage)
        {
            if (rate <= 0)
                throw new ArgumentException("Rate must be greater than zero", nameof(rate));
                
            if (spreadPercentage < 0)
                throw new ArgumentException("Spread percentage cannot be negative", nameof(spreadPercentage));
                
            Id = id;
            BaseCurrency = baseCurrency;
            QuoteCurrency = quoteCurrency;
            Rate = rate;
            SpreadPercentage = spreadPercentage;
            LastUpdated = DateTime.UtcNow;
            
            AddDomainEvent(new ExchangeRateCreatedEvent(
                Id,
                BaseCurrency.Code,
                QuoteCurrency.Code,
                Rate,
                SpreadPercentage));
        }
        
        public void UpdateRate(decimal newRate, decimal newSpreadPercentage)
        {
            if (newRate <= 0)
                throw new ArgumentException("Rate must be greater than zero", nameof(newRate));
                
            if (newSpreadPercentage < 0)
                throw new ArgumentException("Spread percentage cannot be negative", nameof(newSpreadPercentage));
                
            var oldRate = Rate;
            var oldSpread = SpreadPercentage;
            
            Rate = newRate;
            SpreadPercentage = newSpreadPercentage;
            LastUpdated = DateTime.UtcNow;
            
            AddDomainEvent(new ExchangeRateUpdatedEvent(
                Id,
                BaseCurrency.Code,
                QuoteCurrency.Code,
                oldRate,
                newRate,
                oldSpread,
                newSpreadPercentage));
        }
        
        public Money Convert(Money amount, bool isBuying)
        {
            if (amount.Currency != BaseCurrency && amount.Currency != QuoteCurrency)
                throw new ArgumentException("Invalid currency for conversion");
                
            var spread = SpreadPercentage / 100;
            var adjustedRate = isBuying ? Rate * (1 + spread) : Rate * (1 - spread);
            
            if (amount.Currency == BaseCurrency)
            {
                return new Money(amount.Amount * adjustedRate, QuoteCurrency);
            }
            else
            {
                return new Money(amount.Amount / adjustedRate, BaseCurrency);
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
}