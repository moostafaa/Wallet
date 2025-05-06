using WalletService.Domain.Common;

namespace WalletService.Domain.StateMachines
{
    public enum KycState
    {
        Submitted,
        Verified,
        Rejected
    }
    
    public class KycRequestState : StateTransition<KycState>
    {
        protected override void ConfigureTransitions()
        {
            Allow(KycState.Submitted, KycState.Verified);
            Allow(KycState.Submitted, KycState.Rejected);
            Allow(KycState.Rejected, KycState.Submitted);
            Allow(KycState.Verified, KycState.Rejected);
        }
    }
}