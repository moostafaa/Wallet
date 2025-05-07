using System;

namespace WalletService.Domain.Events
{
    public class DisputeStateChangedEvent : DomainEvent
    {
        public Guid DisputeId { get; }
        public string PreviousState { get; }
        public string NewState { get; }
        public string Reason { get; }

        public DisputeStateChangedEvent(
            Guid disputeId,
            string previousState,
            string newState,
            string reason)
        {
            DisputeId = disputeId;
            PreviousState = previousState;
            NewState = newState;
            Reason = reason;
        }
    }

    public class KycStateChangedEvent : DomainEvent
    {
        public Guid AccountId { get; }
        public string PreviousState { get; }
        public string NewState { get; }
        public string DocumentReference { get; }

        public KycStateChangedEvent(
            Guid accountId,
            string previousState,
            string newState,
            string documentReference)
        {
            AccountId = accountId;
            PreviousState = previousState;
            NewState = newState;
            DocumentReference = documentReference;
        }
    }

    public class TopUpStateChangedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public string PreviousState { get; }
        public string NewState { get; }
        public string Reason { get; }

        public TopUpStateChangedEvent(
            Guid requestId,
            string previousState,
            string newState,
            string reason)
        {
            RequestId = requestId;
            PreviousState = previousState;
            NewState = newState;
            Reason = reason;
        }
    }

    public class TransferStateChangedEvent : DomainEvent
    {
        public Guid TransferId { get; }
        public string PreviousState { get; }
        public string NewState { get; }
        public string Reason { get; }

        public TransferStateChangedEvent(
            Guid transferId,
            string previousState,
            string newState,
            string reason)
        {
            TransferId = transferId;
            PreviousState = previousState;
            NewState = newState;
            Reason = reason;
        }
    }

    public class WalletAccountStateChangedEvent : DomainEvent
    {
        public Guid AccountId { get; }
        public string PreviousState { get; }
        public string NewState { get; }
        public string Reason { get; }

        public WalletAccountStateChangedEvent(
            Guid accountId,
            string previousState,
            string newState,
            string reason)
        {
            AccountId = accountId;
            PreviousState = previousState;
            NewState = newState;
            Reason = reason;
        }
    }

    public class WalletTransactionStateChangedEvent : DomainEvent
    {
        public Guid TransactionId { get; }
        public string PreviousState { get; }
        public string NewState { get; }
        public string Reason { get; }

        public WalletTransactionStateChangedEvent(
            Guid transactionId,
            string previousState,
            string newState,
            string reason)
        {
            TransactionId = transactionId;
            PreviousState = previousState;
            NewState = newState;
            Reason = reason;
        }
    }

    public class WithdrawalStateChangedEvent : DomainEvent
    {
        public Guid RequestId { get; }
        public string PreviousState { get; }
        public string NewState { get; }
        public string Reason { get; }

        public WithdrawalStateChangedEvent(
            Guid requestId,
            string previousState,
            string newState,
            string reason)
        {
            RequestId = requestId;
            PreviousState = previousState;
            NewState = newState;
            Reason = reason;
        }
    }
}