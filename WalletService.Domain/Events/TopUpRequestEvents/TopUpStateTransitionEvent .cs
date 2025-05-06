using System;
using WalletService.Domain.StateMachines;
namespace WalletService.Domain.Events.TopUpRequestEvents
{
    public class TopUpStateTransitionEvent : StateTransitionEvent
    {
        public TopUpStateTransitionEvent(
            Guid requestId, 
            TopUpState fromState, 
            TopUpState toState, 
            string reason = null) 
            : base(requestId, fromState.ToString(), toState.ToString(), reason)
        {
        }
    }
    public class TopUpProcessingStartedEvent : TopUpStateTransitionEvent
    {
        public TopUpProcessingStartedEvent(Guid requestId) 
            : base(requestId, TopUpState.Initiated, TopUpState.Processing)
        {
        }
    }
    public class TopUpCompletedEvent : TopUpStateTransitionEvent
    {
        public TopUpCompletedEvent(Guid requestId) 
            : base(requestId, TopUpState.Processing, TopUpState.Success)
        {
        }
    }
    public class TopUpFailedEvent : TopUpStateTransitionEvent
    {
        public string ErrorCode { get; }
        public TopUpFailedEvent(Guid requestId, string reason, string errorCode) 
            : base(requestId, TopUpState.Processing, TopUpState.Failed, reason)
        {
            ErrorCode = errorCode;
        }
    }
}