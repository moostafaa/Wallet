using System;
using WalletService.Domain.Events;
using WalletService.Domain.ValueObjects;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class RecurringTransfer
    {
        public Guid Id { get; private set; }
        public Guid SourceWalletId { get; private set; }
        public Guid DestinationWalletId { get; private set; }
        public Money Amount { get; private set; }
        public string Description { get; private set; }
        public RecurringTransferFrequency Frequency { get; private set; }
        public DateTime NextExecutionDate { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastExecutionDate { get; private set; }
        
        private RecurringTransfer() { }
        
        public RecurringTransfer(
            Guid id,
            Guid sourceWalletId,
            Guid destinationWalletId,
            Money amount,
            string description,
            RecurringTransferFrequency frequency,
            DateTime firstExecutionDate)
        {
            Id = id;
            SourceWalletId = sourceWalletId;
            DestinationWalletId = destinationWalletId;
            Amount = amount;
            Description = description;
            Frequency = frequency;
            NextExecutionDate = firstExecutionDate;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new RecurringTransferCreatedEvent(
                Id,
                SourceWalletId,
                DestinationWalletId,
                Amount.Amount,
                Amount.Currency.Code,
                Description,
                Frequency.ToString(),
                NextExecutionDate));
        }
        
        public void Execute(Guid transactionId)
        {
            if (!IsActive)
                throw new InvalidOperationException("Cannot execute inactive recurring transfer");
                
            LastExecutionDate = DateTime.UtcNow;
            NextExecutionDate = CalculateNextExecutionDate();
            
            AddDomainEvent(new RecurringTransferExecutedEvent(
                Id,
                transactionId,
                NextExecutionDate));
        }
        
        public void Deactivate()
        {
            if (!IsActive)
                throw new InvalidOperationException("Recurring transfer is already inactive");
                
            IsActive = false;
            
            AddDomainEvent(new RecurringTransferDeactivatedEvent(Id));
        }
        
        private DateTime CalculateNextExecutionDate()
        {
            return Frequency switch
            {
                RecurringTransferFrequency.Daily => NextExecutionDate.AddDays(1),
                RecurringTransferFrequency.Weekly => NextExecutionDate.AddDays(7),
                RecurringTransferFrequency.Monthly => NextExecutionDate.AddMonths(1),
                _ => throw new InvalidOperationException($"Unsupported frequency: {Frequency}")
            };
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
    
    public enum RecurringTransferFrequency
    {
        Daily,
        Weekly,
        Monthly
    }
}