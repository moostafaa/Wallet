using System;
using System.Collections.Generic;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class PaymentRequest
    {
        public Guid Id { get; private set; }
        public Guid BuyerWalletId { get; private set; }
        public Guid MerchantWalletId { get; private set; }
        public Money Amount { get; private set; }
        public string OrderReference { get; private set; }
        public string Description { get; private set; }
        public PaymentStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public Guid? TransactionId { get; private set; }
        
        private PaymentRequest() { }
        
        public PaymentRequest(
            Guid id,
            Guid buyerWalletId,
            Guid merchantWalletId,
            Money amount,
            string orderReference,
            string description)
        {
            Id = id;
            BuyerWalletId = buyerWalletId;
            MerchantWalletId = merchantWalletId;
            Amount = amount;
            OrderReference = orderReference;
            Description = description;
            Status = PaymentStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new PaymentRequestCreatedEvent(
                Id,
                BuyerWalletId,
                MerchantWalletId,
                Amount.Amount,
                Amount.Currency.Code,
                OrderReference,
                Description));
        }
        
        public void Complete(Guid transactionId)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Can only complete pending payments");
                
            Status = PaymentStatus.Completed;
            CompletedAt = DateTime.UtcNow;
            TransactionId = transactionId;
            
            AddDomainEvent(new PaymentCompletedEvent(
                Id,
                TransactionId.Value,
                CompletedAt.Value));
        }
        
        public void Fail(string reason)
        {
            if (Status != PaymentStatus.Pending)
                throw new InvalidOperationException("Can only fail pending payments");
                
            Status = PaymentStatus.Failed;
            CompletedAt = DateTime.UtcNow;
            
            AddDomainEvent(new PaymentFailedEvent(Id, reason));
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
    
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
}