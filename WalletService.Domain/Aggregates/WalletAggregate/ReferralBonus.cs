using System;
using System.Collections.Generic;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class ReferralBonus
    {
        public Guid Id { get; private set; }
        public Guid ReferrerId { get; private set; }
        public Guid ReferredUserId { get; private set; }
        public Money BonusAmount { get; private set; }
        public ReferralBonusStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? PaidAt { get; private set; }
        public Guid? TransactionId { get; private set; }
        
        private ReferralBonus() { }
        
        public ReferralBonus(
            Guid id,
            Guid referrerId,
            Guid referredUserId,
            Money bonusAmount)
        {
            Id = id;
            ReferrerId = referrerId;
            ReferredUserId = referredUserId;
            BonusAmount = bonusAmount;
            Status = ReferralBonusStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new ReferralBonusCreatedEvent(
                Id,
                ReferrerId,
                ReferredUserId,
                BonusAmount.Amount,
                BonusAmount.Currency.Code));
        }
        
        public void MarkAsPaid(Guid transactionId)
        {
            if (Status != ReferralBonusStatus.Pending)
                throw new InvalidOperationException("Can only pay pending referral bonuses");
                
            Status = ReferralBonusStatus.Paid;
            PaidAt = DateTime.UtcNow;
            TransactionId = transactionId;
            
            AddDomainEvent(new ReferralBonusPaidEvent(
                Id,
                ReferrerId,
                TransactionId.Value));
        }
        
        public void Cancel(string reason)
        {
            if (Status != ReferralBonusStatus.Pending)
                throw new InvalidOperationException("Can only cancel pending referral bonuses");
                
            Status = ReferralBonusStatus.Cancelled;
            
            AddDomainEvent(new ReferralBonusCancelledEvent(
                Id,
                reason));
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
    
    public enum ReferralBonusStatus
    {
        Pending,
        Paid,
        Cancelled
    }
}