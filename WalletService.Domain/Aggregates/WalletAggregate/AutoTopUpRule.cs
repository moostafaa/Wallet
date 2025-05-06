using System;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class AutoTopUpRule
    {
        public Guid Id { get; private set; }
        public Guid WalletId { get; private set; }
        public Money TriggerAmount { get; private set; }
        public Money TopUpAmount { get; private set; }
        public string Source { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        
        private AutoTopUpRule() { }
        
        public AutoTopUpRule(
            Guid id,
            Guid walletId,
            Money triggerAmount,
            Money topUpAmount,
            string source)
        {
            if (triggerAmount.Currency != topUpAmount.Currency)
                throw new InvalidOperationException("Trigger and top-up amounts must be in the same currency");
                
            Id = id;
            WalletId = walletId;
            TriggerAmount = triggerAmount;
            TopUpAmount = topUpAmount;
            Source = source;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new AutoTopUpRuleCreatedEvent(
                Id,
                WalletId,
                TriggerAmount.Amount,
                TopUpAmount.Amount,
                Source));
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