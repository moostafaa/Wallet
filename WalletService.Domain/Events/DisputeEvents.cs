using System;

namespace WalletService.Domain.Events
{
    public class DisputeCreatedEvent : DomainEvent
    {
        public Guid DisputeId { get; }
        public Guid TransactionId { get; }
        public Guid WalletId { get; }
        public string Reason { get; }
        public string Evidence { get; }
        
        public DisputeCreatedEvent(
            Guid disputeId,
            Guid transactionId,
            Guid walletId,
            string reason,
            string evidence)
        {
            DisputeId = disputeId;
            TransactionId = transactionId;
            WalletId = walletId;
            Reason = reason;
            Evidence = evidence;
        }
    }
    
    public class DisputeNoteAddedEvent : DomainEvent
    {
        public Guid DisputeId { get; }
        public string AdminId { get; }
        public string Note { get; }
        
        public DisputeNoteAddedEvent(
            Guid disputeId,
            string adminId,
            string note)
        {
            DisputeId = disputeId;
            AdminId = adminId;
            Note = note;
        }
    }
    
    public class DisputeApprovedEvent : DomainEvent
    {
        public Guid DisputeId { get; }
        public string AdminId { get; }
        public Guid RefundTransactionId { get; }
        
        public DisputeApprovedEvent(
            Guid disputeId,
            string adminId,
            Guid refundTransactionId)
        {
            DisputeId = disputeId;
            AdminId = adminId;
            RefundTransactionId = refundTransactionId;
        }
    }
    
    public class DisputeRejectedEvent : DomainEvent
    {
        public Guid DisputeId { get; }
        public string AdminId { get; }
        public string Reason { get; }
        
        public DisputeRejectedEvent(
            Guid disputeId,
            string adminId,
            string reason)
        {
            DisputeId = disputeId;
            AdminId = adminId;
            Reason = reason;
        }
    }
    
    public class DisputeEscalatedEvent : DomainEvent
    {
        public Guid DisputeId { get; }
        public string AdminId { get; }
        public string Reason { get; }
        
        public DisputeEscalatedEvent(
            Guid disputeId,
            string adminId,
            string reason)
        {
            DisputeId = disputeId;
            AdminId = adminId;
            Reason = reason;
        }
    }
    
    public class ManualRefundIssuedEvent : DomainEvent
    {
        public Guid RefundId { get; }
        public Guid WalletId { get; }
        public decimal Amount { get; }
        public string CurrencyCode { get; }
        public string Reason { get; }
        public string AdminId { get; }
        public Guid TransactionId { get; }
        
        public ManualRefundIssuedEvent(
            Guid refundId,
            Guid walletId,
            decimal amount,
            string currencyCode,
            string reason,
            string adminId,
            Guid transactionId)
        {
            RefundId = refundId;
            WalletId = walletId;
            Amount = amount;
            CurrencyCode = currencyCode;
            Reason = reason;
            AdminId = adminId;
            TransactionId = transactionId;
        }
    }
}