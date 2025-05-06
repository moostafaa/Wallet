using System;
using WalletService.Domain.StateMachines;
namespace WalletService.Domain.Events.WithdrawalRequestEvents
{
    public class WithdrawalStateTransitionEvent : StateTransitionEvent
    {
        public WithdrawalStateTransitionEvent(
            Guid requestId, 
            WithdrawalState fromState, 
            WithdrawalState toState, 
            string reason = null) 
            : base(requestId, fromState.ToString(), toState.ToString(), reason)
        {
        }
    }
    public class WithdrawalRequestApprovedEvent : WithdrawalStateTransitionEvent
    {
        public string ApprovedBy { get; }
        public WithdrawalRequestApprovedEvent(Guid requestId, string approvedBy) 
            : base(requestId, WithdrawalState.Requested, WithdrawalState.Approved)
        {
            ApprovedBy = approvedBy;
        }
    }
    public class WithdrawalProcessingStartedEvent : WithdrawalStateTransitionEvent
    {
        public WithdrawalProcessingStartedEvent(Guid requestId) 
            : base(requestId, WithdrawalState.Approved, WithdrawalState.Processing)
        {
        }
    }
    public class WithdrawalCompletedEvent : WithdrawalStateTransitionEvent
    {
        public string TransactionReference { get; }
        public WithdrawalCompletedEvent(Guid requestId, string transactionReference) 
            : base(requestId, WithdrawalState.Processing, WithdrawalState.Success)
        {
            TransactionReference = transactionReference;
        }
    }
    public class WithdrawalFailedEvent : WithdrawalStateTransitionEvent
    {
        public string ErrorCode { get; }
        public WithdrawalFailedEvent(Guid requestId, string reason, string errorCode) 
            : base(requestId, WithdrawalState.Processing, WithdrawalState.Failed, reason)
        {
            ErrorCode = errorCode;
        }
    }
}