using System;
using System.Collections.Generic;
using WalletService.Domain.Events;

namespace WalletService.Domain.Aggregates.WalletAggregate
{
    public class DisputeRequest
    {
        public Guid Id { get; private set; }
        public Guid TransactionId { get; private set; }
        public Guid WalletId { get; private set; }
        public string Reason { get; private set; }
        public string Evidence { get; private set; }
        public DisputeStatus Status { get; private set; }
        public string AdminNotes { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ResolvedAt { get; private set; }
        public string ResolvedBy { get; private set; }
        public Guid? RefundTransactionId { get; private set; }
        
        private DisputeRequest() { }
        
        public DisputeRequest(
            Guid id,
            Guid transactionId,
            Guid walletId,
            string reason,
            string evidence)
        {
            Id = id;
            TransactionId = transactionId;
            WalletId = walletId;
            Reason = reason;
            Evidence = evidence;
            Status = DisputeStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            
            AddDomainEvent(new DisputeCreatedEvent(
                Id,
                TransactionId,
                WalletId,
                Reason,
                Evidence));
        }
        
        public void AddAdminNote(string note, string adminId)
        {
            if (Status != DisputeStatus.Pending)
                throw new InvalidOperationException("Can only add notes to pending disputes");
                
            AdminNotes = string.IsNullOrEmpty(AdminNotes) 
                ? $"{adminId}: {note}" 
                : $"{AdminNotes}\n{adminId}: {note}";
                
            AddDomainEvent(new DisputeNoteAddedEvent(Id, adminId, note));
        }
        
        public void Approve(string adminId, Guid refundTransactionId)
        {
            if (Status != DisputeStatus.Pending)
                throw new InvalidOperationException("Can only approve pending disputes");
                
            Status = DisputeStatus.Approved;
            ResolvedAt = DateTime.UtcNow;
            ResolvedBy = adminId;
            RefundTransactionId = refundTransactionId;
            
            AddDomainEvent(new DisputeApprovedEvent(
                Id,
                adminId,
                RefundTransactionId.Value));
        }
        
        public void Reject(string adminId, string reason)
        {
            if (Status != DisputeStatus.Pending)
                throw new InvalidOperationException("Can only reject pending disputes");
                
            Status = DisputeStatus.Rejected;
            ResolvedAt = DateTime.UtcNow;
            ResolvedBy = adminId;
            AdminNotes = string.IsNullOrEmpty(AdminNotes)
                ? $"Rejection reason: {reason}"
                : $"{AdminNotes}\nRejection reason: {reason}";
                
            AddDomainEvent(new DisputeRejectedEvent(
                Id,
                adminId,
                reason));
        }
        
        public void Escalate(string adminId, string reason)
        {
            if (Status != DisputeStatus.Pending)
                throw new InvalidOperationException("Can only escalate pending disputes");
                
            Status = DisputeStatus.Escalated;
            AdminNotes = string.IsNullOrEmpty(AdminNotes)
                ? $"Escalation reason: {reason}"
                : $"{AdminNotes}\nEscalation reason: {reason}";
                
            AddDomainEvent(new DisputeEscalatedEvent(
                Id,
                adminId,
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
    
    public enum DisputeStatus
    {
        Pending,
        Approved,
        Rejected,
        Escalated
    }
}