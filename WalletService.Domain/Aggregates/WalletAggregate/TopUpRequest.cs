using System;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class TopUpRequest
    {
        public Guid Id { get; private set; }
        public Guid WalletId { get; private set; }
        public Money Amount { get; private set; }
        public string Source { get; private set; }
        public string ExternalReference { get; private set; }
        public TopUpRequestStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        
        private TopUpRequest() { }
        
        public TopUpRequest(
            Guid id,
            Guid walletId,
            Money amount,
            string source,
            string externalReference)
        {
            Id = id;
            WalletId = walletId;
            Amount = amount;
            Source = source;
            ExternalReference = externalReference;
            Status = TopUpRequestStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new TopUpRequestCreatedEvent(
                Id,
                WalletId,
                Amount.Amount,
                Amount.Currency.Code,
                Source,
                ExternalReference));
        }
        
        public void Complete(string transactionReference)
        {
            if (Status != TopUpRequestStatus.Pending)
                throw new InvalidOperationException("Can only complete pending top-up requests");
                
            Status = TopUpRequestStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            
            AddDomainEvent(new TopUpCompletedEvent(
                Id,
                WalletId,
                Amount.Amount,
                Amount.Currency.Code,
                transactionReference));
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
    
    public enum TopUpRequestStatus
    {
        Pending,
        Completed,
        Failed,
        Cancelled
    }
}