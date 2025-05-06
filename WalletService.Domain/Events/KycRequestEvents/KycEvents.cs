using System;
using WalletService.Domain.StateMachines;
namespace WalletService.Domain.Events.KycRequestEvents
{
    public class KycStateTransitionEvent : StateTransitionEvent
    {
        public KycStateTransitionEvent(
            Guid requestId, 
            KycState fromState, 
            KycState toState, 
            string reason = null) 
            : base(requestId, fromState.ToString(), toState.ToString(), reason)
        {
        }
    }
    public class KycVerificationCompletedEvent : KycStateTransitionEvent
    {
        public string VerifiedBy { get; }
        public KycVerificationCompletedEvent(Guid requestId, string verifiedBy) 
            : base(requestId, KycState.Submitted, KycState.Verified)
        {
            VerifiedBy = verifiedBy;
        }
    }
    public class KycVerificationRejectedEvent : KycStateTransitionEvent
    {
        public string RejectedBy { get; }
        public KycVerificationRejectedEvent(Guid requestId, string rejectedBy, string reason) 
            : base(requestId, KycState.Submitted, KycState.Rejected, reason)
        {
            RejectedBy = rejectedBy;
        }
    }
}