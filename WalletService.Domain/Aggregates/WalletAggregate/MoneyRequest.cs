using System;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class MoneyRequest
    {
        public Guid Id { get; private set; }
        public Guid RequesterId { get; private set; }
        public Guid RequesteeId { get; private set; }
        public Money Amount { get; private set; }
        public string Description { get; private set; }
        public MoneyRequestStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public Guid? TransactionId { get; private set; }
        
        private MoneyRequest() { }
        
        public MoneyRequest(
            Guid id,
            Guid requesterId,
            Guid requesteeId,
            Money amount,
            string description)
        {
            Id = id;
            RequesterId = requesterId;
            RequesteeId = requesteeId;
            Amount = amount;
            Description = description;
            Status = MoneyRequestStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new MoneyRequestCreatedEvent(
                Id,
                RequesterId,
                RequesteeId,
                Amount.Amount,
                Amount.Currency.Code,
                Description));
        }
        
        public void Accept(Guid transactionId)
        {
            if (Status != MoneyRequestStatus.Pending)
                throw new InvalidOperationException("Can only accept pending money requests");
                
            Status = MoneyRequestStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            TransactionId = transactionId;
            
            AddDomainEvent(new MoneyRequestAcceptedEvent(
                Id,
                TransactionId.Value));
        }
        
        public void Reject(string reason)
        {
            if (Status != MoneyRequestStatus.Pending)
                throw new InvalidOperationException("Can only reject pending money requests");
                
            Status = MoneyRequestStatus.Rejected;
            CompletedAt = DateTime.UtcNow;
            
            AddDomainEvent(new MoneyRequestRejectedEvent(
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
    
    public enum MoneyRequestStatus
    {
        Pending,
        Completed,
        Rejected,
        Expired
    }
}